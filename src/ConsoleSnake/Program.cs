using Engine;
using Engine.Abstractions;
using Enums.Engine;
using System;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ConsoleSnake
{
    class Program
    {
        static readonly int xMapSize = 20;
        static readonly int yMapSize = 20;
        static readonly IDownloader downloader = new Downloader();
        static Game game;
        static readonly string wallSymbol = "x ";
        static readonly string bodySymbol = "o ";
        static readonly string foodSymbol = "* ";
        static readonly string backgroundSymbol = "  ";
        static Point lastPoint;
        static Status lastStatus;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize((xMapSize + 70) * 2, yMapSize + 10);
            NewGame();
            Show();
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            do
            {
                if (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true);
                    if (key.KeyChar == 'a' || key.KeyChar == 'w' || key.KeyChar == 's' || key.KeyChar == 'd' || key.KeyChar == 'r')
                    {
                        switch (key.KeyChar)
                        {
                            case 'a':
                                game.SetDirection(Direction.Left);
                                break;
                            case 'w':
                                game.SetDirection(Direction.Up);
                                break;
                            case 's':
                                game.SetDirection(Direction.Down);
                                break;
                            case 'd':
                                game.SetDirection(Direction.Right);
                                break;
                            case 'r':
                                if (game.GameStatus == Status.Win || game.GameStatus == Status.Lost)
                                {
                                    NewGame();
                                    game.Start();
                                }

                                if (game.GameStatus == Status.Ready)
                                    game.Start();
                                break;
                        }
                    }
                }
            }
            while (key.KeyChar != 'q');
            game.Stop();
        }
        private static void PrintMap()
        {
            var map = game.Map();
            map.ForEach(point => { Console.SetCursorPosition(point.X * 2, point.Y); Console.Write(wallSymbol); });
        }

        private static void PrintFood()
        {
            var food = game.Food();
            Console.SetCursorPosition(food.X * 2, food.Y);
            Console.Write(foodSymbol);
        }

        private static void PrintBody()
        {
            if (lastPoint != null)
            {
                Console.SetCursorPosition(lastPoint.X * 2, lastPoint.Y);
                Console.Write(backgroundSymbol);
            }
            var body = game.Body();
            body.ForEach(point => { Console.SetCursorPosition(point.X * 2, point.Y); Console.Write(bodySymbol); });
            lastPoint = body.First();
        }

        private static void NewGame()
        {
            Console.Clear();
            var map = new Map(xMapSize, yMapSize);
            var awatar = new Avatar(new Point(10, 10), Direction.Down);
            game = new Game(awatar, map, downloader, () => Show(), () => Downloaded());
            PrintMap();
        }

        private static void Downloaded()
        {
            int i = 0;
            
            game.Downloaded.ToList().ForEach(x => 
            {
                Console.SetCursorPosition(xMapSize * 2 + 5, i++);
                Console.Write(x);
            });
        }

        private static void Show()
        {
            if (game.GameStatus == Status.PointGained || game.GameStatus == Status.Ok)
            {
                PrintFood();
            }
            PrintBody();
            StringBuilder buff = new StringBuilder();
            buff.Append($"\n Current score: {game.Score}");

            if (lastStatus != game.GameStatus)
            {
                Clear();
                switch (game.GameStatus)
                {
                    case Status.Lost:
                        game.Stop();
                        buff.Append("\n Press R to start game, use Q to quit");
                        buff.Append("\n \n GAME LOST!");
                        break;

                    case Status.Win:
                        game.Stop();
                        buff.Append("\n Press R to start game, use Q to quit");
                        buff.Append("\n \n GAME WIN!");
                        break;

                    case Status.PointGained:
                    case Status.Ok:
                        buff.Append("\n Use ASWD to steer");
                        buff.Append("\n Use Q to quit");
                        break;

                    case Status.Ready:
                        buff.Append("\n Use ASWD to steer");
                        buff.Append("\n Press R to start game, use Q to quit");
                        break;
                }
                lastStatus = game.GameStatus;
            }

            Console.SetCursorPosition(0, yMapSize + 2);
            Console.Write(buff.ToString());
        }

        private static void Clear()
        {
            Console.SetCursorPosition(0, yMapSize + 3);
            for (int j = 0; j <= 2; j++)
            {
                for (int i = 0; i <= (xMapSize + 2) * 2; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}
