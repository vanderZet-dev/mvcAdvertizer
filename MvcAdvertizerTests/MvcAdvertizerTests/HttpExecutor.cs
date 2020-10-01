using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcAdvertizerTests
{
    public static class HttpExecutor
    {
        private static readonly HttpClient client = new HttpClient();

        private static async Task<string> PostRequest(string endPoint, Dictionary<string, string> data) {

            var content = new FormUrlEncodedContent(data);
            var response = await client.PostAsync(endPoint, content);
            var statusCode = response.StatusCode;

            return statusCode.ToString();
        }

        private static async Task ExecuteCreateAdvertsWithLock() {

            var userId = "d5e8ce98-8e28-47e8-beb0-2fe32a5c0987";

            var endPoint = "http://localhost:61703/Advert/CreateWithoutRecaptcha";

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

            try
            {
                var statusCode = await PostRequest(endPoint, values);
                Console.WriteLine(statusCode);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void GenerateAdverts() {

            List<Task> TaskList = new List<Task>();

            Console.WriteLine("Выполняется...");

            for (int i = 0; i < 3; i++)
            {
                TaskList.Add(ExecuteCreateAdvertsWithLock());
            }

            Task.WaitAll(TaskList.ToArray());

            Menu.ShowMenu();
        }
    }
}
