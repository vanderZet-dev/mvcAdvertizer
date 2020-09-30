using Microsoft.AspNetCore.Http;
using MvcAdvertizer.Services.Interfaces;
using System;
using System.IO;

namespace MvcAdvertizer.Services.Implementations
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string savePath = Path.Combine("storage");

        public byte[] GetFileData(string hash) {
                        
            byte[] bytes = null;
            string path = Path.Combine(savePath, hash);

            if (File.Exists(path))
            {                
                bytes = File.ReadAllBytes(path);            
            }

            return bytes;
        }

        public string Save(IFormFile file) {

            string newFileName = DateTime.Now.Ticks + "_" + Guid.NewGuid().ToString();
            Directory.CreateDirectory(savePath);
            var filePath = Path.Combine(savePath, newFileName);
                        
            using (var stream = new FileStream(filePath, FileMode.Create))
            {            
                file.CopyTo(stream);
            }

            return newFileName;
        }
    }
}
