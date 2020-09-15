using MvcAdvertizer.Config.Tools;
using Newtonsoft.Json;

namespace MvcAdvertizer.Utils
{
    public static class ToastGeneratorUtils
    {
        public static string CreateSerializedToasterData(string header, string text, string color) {

            var toaster = new Toaster();
            toaster.Color = color;
            toaster.Header = header;
            toaster.Text = text;
            return JsonConvert.SerializeObject(toaster);
        }

        public static string GetSuccessRecordUpdateSerializedToasterData() {           
            
            var header = "Успешно";
            var text = "Запись успешно обновлена!";
            var color = "lightgreen";

            return CreateSerializedToasterData(header, text, color);
        }

        public static string GetSuccessRecordCreateSerializedToasterData() {

            var header = "Успешно";
            var text = "Запись успешно создана!";
            var color = "lightgreen";

            return CreateSerializedToasterData(header, text, color);
        }

        public static string GetSuccessRecordDeleteSerializedToasterData() {

            var header = "Успешно";
            var text = "Запись успешно Удалена!";
            var color = "lightgreen";

            return CreateSerializedToasterData(header, text, color);
        }
    }
}
