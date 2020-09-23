using Microsoft.AspNetCore.Http;
using MvcAdvertizer.Services.Interfaces;
using MvcAdvertizer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                file.CopyToAsync(stream);
            }

            return newFileName;
        }
    }
}
