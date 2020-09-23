using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MvcAdvertizer.Config;
using MvcAdvertizer.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcAdvertizer.Services.Implementations
{
    public class RecaptchaService : IRecaptchaService
    {
        private readonly RecaptchaSettings recaptchaSettings;
        private readonly IHttpClientFactory httpClientFactory;

        public RecaptchaService(IOptions<AppSettings> settings,
                                IHttpClientFactory httpClientFactory) {
            recaptchaSettings = settings?.Value?.RecaptchaSettings;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CheckRecaptcha(string recaptchaResponse, string connectionRemoteIpAddress) {            

            var recaptchaVerifyEndPoint = recaptchaSettings.VerifyEndPoint;
            var recaptchaSecretKey = recaptchaSettings.SecretKey;

            var parameters = new Dictionary<string, string>
                {
                    {"secret", recaptchaSecretKey},
                    {"response", recaptchaResponse},
                    {"remoteip", connectionRemoteIpAddress}
                };

            var valid = await GetRecaptchaCheckResult(recaptchaVerifyEndPoint, parameters);

            return valid;
        }

        private async Task<bool> GetRecaptchaCheckResult(string recaptchaVerifyEndPoint, Dictionary<string, string> parameters) {

            var valid = true;

            var client = httpClientFactory.CreateClient();
            try
            {      
                HttpResponseMessage response = await client.PostAsync(recaptchaVerifyEndPoint, new FormUrlEncodedContent(parameters));
                response.EnsureSuccessStatusCode();

                string apiResponse = await response.Content.ReadAsStringAsync();
                dynamic apiJson = JObject.Parse(apiResponse);
                if (apiJson.success != true)
                {
                    valid = false;
                }
            }
            catch (HttpRequestException ex)
            {
                valid = false;
            }

            return valid;
        }
    }
}
