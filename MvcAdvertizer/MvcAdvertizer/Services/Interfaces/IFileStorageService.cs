using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MvcAdvertizer.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> Save(IFormFile file);

        Task<byte[]> GetFileData(string hash);
    }
}
