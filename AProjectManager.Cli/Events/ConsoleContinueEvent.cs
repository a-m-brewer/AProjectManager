using System;
using AProjectManager.Interfaces;

namespace AProjectManager.Cli.Events
{
    public class ConsoleContinueEvent : IContinueEvent
    {
        public bool Continue(string prompt)
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