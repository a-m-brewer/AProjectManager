using System;

namespace AProjectManager.Utils
{
    public static class ConsoleEvents
    {
        public static bool YesNoInput(string prompt)
        {
            var yes = false;
            var inputComplete = false;

            while (!inputComplete)
            {
                Console.Write($"{prompt} [Y/n]");
                var response = Console.ReadKey(false).Key;

                inputComplete = response == ConsoleKey.Y || response == ConsoleKey.N || response == ConsoleKey.Enter;
                yes = response == ConsoleKey.Y || response == ConsoleKey.Enter;
            }

            return yes;
        }
    }
}