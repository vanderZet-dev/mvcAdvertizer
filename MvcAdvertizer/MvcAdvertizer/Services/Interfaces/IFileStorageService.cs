using Microsoft.AspNetCore.Http;

namespace MvcAdvertizer.Services.Interfaces
{
    public interface IFileStorageService
    {
        string Save(IFormFile file);

        IFormFile GetFile(string hash);
    }
}
