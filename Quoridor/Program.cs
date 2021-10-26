using Quoridor.Model;
using Quoridor.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quoridor.Controller
{
    class Program
    {
        static void Main()
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;

            Console.WindowHeight = 40;
            Console.WindowWidth = 100;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            GameManager gameManager = new GameManager();
            ViewManager viewManager = new ViewManager(gameManager);

            gameManager.Initialize();

            bool run = true;
            while (run)
            {
                run = gameManager.ConsumeInput(GetInputByKey(Console.ReadKey(true).Key));
            }

            GC.KeepAlive(viewManager); // MAYBE COMMENT OUT LATER
        }

        public static Input GetInputByKey(ConsoleKey key)
        {
            return key switch
            {
                ConsoleKey.D0 =>        new Input() { Type = InputType.Number, Number = 0 },
                ConsoleKey.NumPad0 =>   new Input() { Type = InputType.Number, Number = 0 },
                ConsoleKey.D1 =>        new Input() { Type = InputType.Number, Number = 1 },
                ConsoleKey.NumPad1 =>   new Input() { Type = InputType.Number, Number = 1 },
                ConsoleKey.D2 =>        new Input() { Type = InputType.Number, Number = 2 },
                ConsoleKey.NumPad2 =>   new Input() { Type = InputType.Number, Number = 2 },
                ConsoleKey.D3 =>        new Input() { Type = InputType.Number, Number = 3 },
                ConsoleKey.NumPad3 =>   new Input() { Type = InputType.Number, Number = 3 },
                ConsoleKey.D4 =>        new Input() { Type = InputType.Number, Number = 4 },
                ConsoleKey.NumPad4 =>   new Input() { Type = InputType.Number, Number = 4 },
                ConsoleKey.D5 =>        new Input() { Type = InputType.Number, Number = 5 },
                ConsoleKey.NumPad5 =>   new Input() { Type = InputType.Number, Number = 5 },
                ConsoleKey.D6 =>        new Input() { Type = InputType.Number, Number = 6 },
                ConsoleKey.NumPad6 =>   new Input() { Type = InputType.Number, Number = 6 },
                ConsoleKey.D7 =>        new Input() { Type = InputType.Number, Number = 7 },
                ConsoleKey.NumPad7 =>   new Input() { Type = InputType.Number, Number = 7 },
                ConsoleKey.D8 =>        new Input() { Type = InputType.Number, Number = 8 },
                ConsoleKey.NumPad8 =>   new Input() { Type = InputType.Number, Number = 8 },
                ConsoleKey.D9 =>        new Input() { Type = InputType.Number, Number = 9 },
                ConsoleKey.NumPad9 =>   new Input() { Type = InputType.Number, Number = 9 },
                ConsoleKey.W =>     new Input() { Type = InputType.Direction, Direction = Direction.Up },
                ConsoleKey.S =>     new Input() { Type = InputType.Direction, Direction = Direction.Down },
                ConsoleKey.A =>     new Input() { Type = InputType.Direction, Direction = Direction.Left },
                ConsoleKey.D =>     new Input() { Type = InputType.Direction, Direction = Direction.Right },
                ConsoleKey.UpArrow =>       new Input() { Type = InputType.Direction, Direction = Direction.Up },
                ConsoleKey.DownArrow =>     new Input() { Type = InputType.Direction, Direction = Direction.Down },
                ConsoleKey.LeftArrow =>     new Input() { Type = InputType.Direction, Direction = Direction.Left },
                ConsoleKey.RightArrow =>    new Input() { Type = InputType.Direction, Direction = Direction.Right },
                _ => new Input() { Type = InputType.Key, Key = key },
            };
        }
    }
}
