using System.Threading.Tasks;

namespace MvcAdvertizer.Services.Interfaces
{
    public interface IRecaptchaService
    {
        public Task<bool> CheckRecaptcha(string recaptchaResponse, string connectionRemoteIpAddress);
    }
}
