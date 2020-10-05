using Microsoft.AspNetCore.Http;
using MvcAdvertizer.Services.Interfaces;
using MvcAdvertizer.Utils;
using System.Threading.Tasks;

namespace MvcAdvertizer.Config.Middlewares
{
    public class ImageResizeMiddleware
    {
        private readonly IFileStorageService fileStorageService;

        private RequestDelegate _next;
        public ImageResizeMiddleware(RequestDelegate next,
                                     IFileStorageService fileStorageService) {
            _next = next;
            this.fileStorageService = fileStorageService;
        }
        public async Task InvokeAsync(HttpContext context) {

            byte[] image = null;
            var width = context.Request.Query["width"];
            var imageHash = context.Request.Query["imageHash"];

            if (string.IsNullOrWhiteSpace(imageHash))
            {
                await _next.Invoke(context);
            }
            else
            {
                image = await fileStorageService.GetFileData(imageHash);

                if (!string.IsNullOrWhiteSpace(width))
                {
                    image = ImageResizerUtill.ScaledByWidth(image, width);

                }

                await context.Response.Body.WriteAsync(image);
            }
        }
    }
}
