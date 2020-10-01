using System;

namespace MvcAdvertizerTests
{
    public static class Menu
    {
        public static void ShowMenu() {

            while (true)
            {
                ConsoleTool.WriteLineConsoleGreenMessage("1 - запустить запрос на создание объявлений.");
                ConsoleTool.WriteLineConsoleGreenMessage("Любая другая клавиша - завершение работы.");

                var cki = Console.ReadKey();
                Console.WriteLine();
                switch (cki.KeyChar.ToString())
                {
                    case "1":
                        HttpExecutor.GenerateAdverts();
                        break;
                    default:
                        Environment.Exit(0);
                        return;
                }
            }
        }
    }
}
