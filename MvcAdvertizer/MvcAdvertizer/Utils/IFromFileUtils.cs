using Microsoft.AspNetCore.Http;
using System.IO;

namespace MvcAdvertizer.Utils
{
    public static class IFromFileUtils
    {
        public static byte[] IFormFileToByteArray(IFormFile file) {

            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    return fileBytes;
                }
            }
            return null;
        }

        public static IFormFile ByteArrayToIFormFile(byte[] bytes) {

            IFormFile file = null;
            
            var stream = new MemoryStream(bytes);
            file = new FormFile(stream, 0, bytes.Length, "name", "fileName");            

            return file;
        }
    }
}
