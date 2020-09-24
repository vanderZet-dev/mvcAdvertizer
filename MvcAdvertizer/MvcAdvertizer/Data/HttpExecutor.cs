using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcAdvertizer.Data
{
    public static class HttpExecutor
    {
        private static readonly HttpClient client = new HttpClient();

        public async static void ExecuteCreateAdvertsWithLock() {

            var endPoint = "http://localhost:61703/Advert/CreateTestWithLock";

            await PostRequest(endPoint);
        }

        public async static void ExecuteCreateAdvertsWithoutLock() {

            var endPoint = "http://localhost:61703/Advert/CreateTestWithoutLock";

            await PostRequest(endPoint);
        }


        private static async Task<string> PostRequest(string endPoint) {

            var userId = Guid.Parse("7069b179-a4ce-43bd-aa0e-dea0a111c6ab");
            Random rng = new Random();
            var number = rng.Next(10000);
            var datetime = DateTime.Now;
            datetime = datetime.AddTicks(-(datetime.Ticks % TimeSpan.TicksPerSecond));           

            var values = new Dictionary<string, string>
            {
                { "AdvertDto.Number", number.ToString() },
                { "AdvertDto.Content", "Тестовое содержание объявления " + number },
                { "AdvertDto.UserId", userId.ToString() },
                { "AdvertDto.Rate", rng.Next(11).ToString() },
                { "PublishingDate", datetime.ToLongDateString() },
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(endPoint, content);

            var responseString = await response.Content.ReadAsStringAsync();            

            return responseString;
        }
    }
}
