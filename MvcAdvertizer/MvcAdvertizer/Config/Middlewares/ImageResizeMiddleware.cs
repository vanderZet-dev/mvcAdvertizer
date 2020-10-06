using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MvcAdvertizer.Services.Interfaces;
using MvcAdvertizer.Utils;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Config.Middlewares
{
    public class ImageResizeMiddleware
    {
        private readonly IFileStorageService fileStorageService;
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment env;

        public ImageResizeMiddleware(RequestDelegate next,
                                     IFileStorageService fileStorageService,
                                     IWebHostEnvironment env) {
            this.next = next;
            this.fileStorageService = fileStorageService;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext context) {

            var path = context.Request.Path;

            if (!IsImagePath(path))
            {
                await next.Invoke(context);
                return;
            }

            var width = context.Request.Query["width"].FirstOrDefault();
            var widthModificator = GenerateWidthModificator(width);

            var imageFullPath = GenerateFullImagePath(path, widthModificator);

            var fileExists = File.Exists(imageFullPath);
            if (fileExists)
            {
                var existedImage = await File.ReadAllBytesAsync(imageFullPath);
                await context.Response.Body.WriteAsync(existedImage);
                return;
            }

            var image = await GetImageFromStorage(path);
            image = ModifyImage(image, width);

            SaveImageToStaticFiles(imageFullPath, image);

            await context.Response.Body.WriteAsync(image);
            return;
        }

        private string GenerateWidthModificator(string width) {

            var widthModificator = "";

            if (!string.IsNullOrWhiteSpace(width))
            {
                widthModificator = string.Concat("width", width);
            }

            return widthModificator;
        }

        private bool IsImagePath(PathString path) {

            return path.StartsWithSegments("/img", StringComparison.OrdinalIgnoreCase);
        }

        private string GenerateFullImagePath(PathString path, string widthModificator) {

            var webRootPath = env.WebRootPath;
            var additionalPath = path.Value.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);

            var generatedFileName = GenerateFileName(additionalPath, widthModificator);

            var splittedAddPath = additionalPath.Split(Path.DirectorySeparatorChar);

            var generatedFullPath = webRootPath;
            for (int i = 0; i < splittedAddPath.Length - 1; i++)
            {
                generatedFullPath = Path.Combine(generatedFullPath, splittedAddPath[i]);
            }
            generatedFullPath = Path.Combine(generatedFullPath, generatedFileName);

            return generatedFullPath;
        }

        private string GenerateFileName(string imagePath, string widthModificator) {

            var fullFileName = GetLastSplittedElement(imagePath, Path.DirectorySeparatorChar.ToString());

            var fileName = GetFirstSplittedElement(fullFileName, ".");

            var fileExtension = GetLastSplittedElement(fullFileName, ".");

            var generatedName = string.Concat(fileName, widthModificator, ".", fileExtension);

            return generatedName;
        }

        private string GetLastSplittedElement(string fullString, string splitByChar) {

            var splitted = fullString.Split(splitByChar);
            var lastElement = splitted.Length - 1;

            if (lastElement <= 0)
            {
                throw new Exception("Can not identify extension type.");
            }

            return splitted[lastElement];
        }

        private string GetFirstSplittedElement(string fullString, string splitByChar) {

            var splitted = fullString.Split(splitByChar);

            if (splitted.Length <= 0)
            {
                throw new Exception("Can not identify extension type.");
            }

            return splitted[0];
        }

        private async Task<byte[]> GetImageFromStorage(PathString path) {

            var imageHash = path.Value.Split("/")[2];
            return await fileStorageService.GetFileData(imageHash);
        }

        private byte[] ModifyImage(byte[] image, string width) {

            if (!string.IsNullOrWhiteSpace(width))
            {
                image = ImageResizerUtill.ScaledByWidth(image, width);
            }

            return image;
        }

        private bool SaveImageToStaticFiles(string fullSavePath, byte[] byteArray) {

            if (byteArray == null)
            {
                throw new Exception("File is not found");
            }

            try
            {
                using (var fs = new FileStream(fullSavePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }
    }
}
