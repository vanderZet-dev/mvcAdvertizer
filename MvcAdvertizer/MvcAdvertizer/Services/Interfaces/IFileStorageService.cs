using Microsoft.AspNetCore.Http;

namespace MvcAdvertizer.Services.Interfaces
{
    public interface IFileStorageService
    {
        string Save(IFormFile file);

        byte[] GetFileData(string hash);
    }
}
