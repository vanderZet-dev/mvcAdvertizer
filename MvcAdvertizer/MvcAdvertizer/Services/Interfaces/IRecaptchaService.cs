using System.Threading.Tasks;

namespace MvcAdvertizer.Services.Interfaces
{
    public interface IRecaptchaService
    {
        public Task<bool> checkRecaptcha(string recaptchaResponse, string connectionRemoteIpAddress);
    }
}
