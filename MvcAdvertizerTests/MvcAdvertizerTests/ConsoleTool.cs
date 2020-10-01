using System;

namespace MvcAdvertizerTests
{
    public static class ConsoleTool
    {
        public static void WriteLineConsoleGreenMessage(string message) {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
