using Microsoft.AspNetCore.Http;
using MvcAdvertizer.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MvcAdvertizer.Services.Implementations
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string savePath = Path.Combine("storage");

        public async Task<byte[]> GetFileData(string hash) {

            byte[] bytes = null;
            string path = Path.Combine(savePath, hash);

            if (File.Exists(path))
            {
                bytes = await File.ReadAllBytesAsync(path);
            }

            return bytes;
        }

        public async Task<string> Save(IFormFile file) {

            string newFileName = DateTime.Now.Ticks + "_" + Guid.NewGuid().ToString();
            Directory.CreateDirectory(savePath);
            var filePath = Path.Combine(savePath, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return newFileName;
        }
    }
}
