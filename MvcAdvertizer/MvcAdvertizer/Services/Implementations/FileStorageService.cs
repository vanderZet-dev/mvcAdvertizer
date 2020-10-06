using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MvcAdvertizer.Config;
using MvcAdvertizer.Core.Exceptions;
using MvcAdvertizer.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MvcAdvertizer.Services.Implementations
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string savePath;

        public FileStorageService(IOptions<FileStorageSettings> settings) {

            savePath = settings?.Value?.BasePath;
        }

        public async Task<byte[]> GetFileData(string hash) {

            SavePathValidation();

            byte[] bytes = null;
            string path = Path.Combine(savePath, hash);

            if (File.Exists(path))
            {
                bytes = await File.ReadAllBytesAsync(path);
            }

            return bytes;
        }

        public async Task<string> Save(IFormFile file) {

            SavePathValidation();

            string extension = ParseFileExtension(file.FileName);

            string newFileName = DateTime.Now.Ticks + "_" + Guid.NewGuid().ToString() + "." + extension;
            Directory.CreateDirectory(savePath);
            var filePath = Path.Combine(savePath, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return newFileName;
        }

        private void SavePathValidation() {

            if (savePath == null || savePath == "")
            {
                throw new FileStorageSavePathInvalidException(savePath);
            }
        }

        private string ParseFileExtension(string fileName) {

            var splitted = fileName.Split(".");
            var lastElement = splitted.Length - 1;

            if (lastElement <= 0)
            {
                throw new Exception("Can not identify extension type.");
            }

            var extension = splitted[lastElement];

            return extension;
        }
    }
}
