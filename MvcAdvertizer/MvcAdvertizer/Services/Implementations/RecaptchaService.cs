using Microsoft.Extensions.Configuration;
using MvcAdvertizer.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcAdvertizer.Services.Implementations
{
    public class RecaptchaService : IRecaptchaService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;

        public RecaptchaService(IConfiguration configuration, 
                                IHttpClientFactory httpClientFactory) {
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<bool> checkRecaptcha(string recaptchaResponse, string connectionRemoteIpAddress) {            

            var recaptchaVerifyEndPoint = configuration.GetSection("Recaptcha").GetSection("recaptchaVerifyEndPoint").Value;
            var recaptchaSecretKey = configuration.GetSection("Recaptcha").GetSection("googleReCaptcha:SecretKey").Value;

            var parameters = new Dictionary<string, string>
                {
                    {"secret", recaptchaSecretKey},
                    {"response", recaptchaResponse},
                    {"remoteip", connectionRemoteIpAddress}
                };

            var valid = await getRecaptchaCheckResult(recaptchaVerifyEndPoint, parameters);

            return valid;
        }

        private async Task<bool> getRecaptchaCheckResult(string recaptchaVerifyEndPoint, Dictionary<string, string> parameters) {

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
