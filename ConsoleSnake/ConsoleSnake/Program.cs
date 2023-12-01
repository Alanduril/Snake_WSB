using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleSnake
{
    class Program
    {
        static void Main(string[] args)
        {
            var snake1 = new List<Point> { new Point(10, 5) }; 
            var snake2 = new List<Point> { new Point(30, 15) };

            var food = GetFood(snake1.Concat(snake2).ToList(), 50, 20); 
            var direction1 = Direction.Right;
            var direction2 = Direction.Left;
            var boardWidth = 50; 
            var boardHeight = 20;
            var score1 = 0;
            var score2 = 0;
            var gameOver = false;

            Console.CursorVisible = false;
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    direction1 = ChangeDirection(key, direction1, true);
                    direction2 = ChangeDirection(key, direction2, false);
                }

                MoveSnake(snake1, direction1);
                MoveSnake(snake2, direction2);

                if (snake1[0].Equals(food))
                {
                    snake1.Add(new Point(snake1[^1].X, snake1[^1].Y));
                    food = GetFood(snake1.Concat(snake2).ToList(), boardWidth, boardHeight);
                    score1++; 
                }
                else if (snake2[0].Equals(food))
                {
                    snake2.Add(new Point(snake2[^1].X, snake2[^1].Y));
                    food = GetFood(snake1.Concat(snake2).ToList(), boardWidth, boardHeight);
                    score2++; 
                }

                if (CheckCollision(snake1) || CheckCollision(snake2))
                {
                    Console.Clear();
                    Console.SetCursorPosition(boardWidth / 2 - 10, boardHeight / 2);
                    Console.Write("Collision! Scores - Player1: {0}, Player2: {1}", score1, score2);
                    Thread.Sleep(2000); // Opóźnienie przed kontynuacją

                    // Resetowanie pozycji węży i wyników
                    snake1.Clear();
                    snake1.Add(new Point(10, 5));
                    snake2.Clear();
                    snake2.Add(new Point(30, 15));
                    score1 = 0;
                    score2 = 0;
                    food = GetFood(snake1.Concat(snake2).ToList(), boardWidth, boardHeight);
                    direction1 = Direction.Right;
                    direction2 = Direction.Left;
                    continue; // Kontynuacja pętli gry
                }

                Console.Clear();
                DrawBoard(snake1, snake2, food, boardWidth, boardHeight);
                Console.WriteLine($"Score1: {score1} Score2: {score2}");
                Thread.Sleep(200);
            }

            Console.SetCursorPosition(boardWidth / 2, boardHeight / 2);
            Console.Write("Game Over! Final Scores - Player1: {0}, Player2: {1}", score1, score2);
        }

        static Point GetFood(List<Point> snake, int width, int height)
        {
            var random = new Random();
            Point food;
            do
            {
                food = new Point(random.Next(width), random.Next(height));
            }
            while (snake.Any(s => s.Equals(food)));
            return food;
        }

        static void MoveSnake(List<Point> snake, Direction direction)
        {
            for (int i = snake.Count - 1; i > 0; i--)
            {
                snake[i] = snake[i - 1];
            }

            snake[0] = direction switch
            {
                Direction.Up => new Point(snake[0].X, snake[0].Y - 1),
                Direction.Down => new Point(snake[0].X, snake[0].Y + 1),
                Direction.Left => new Point(snake[0].X - 1, snake[0].Y),
                Direction.Right => new Point(snake[0].X + 1, snake[0].Y),
                _ => snake[0]
            };
        }

        static void DrawBoard(List<Point> snake1, List<Point> snake2, Point food, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (snake1.Any(s => s.X == x && s.Y == y))
                    {
                        Console.Write("*");
                    }
                    else if (snake2.Any(s => s.X == x && s.Y == y))
                    {
                        Console.Write("+");
                    }
                    else if (food.X == x && food.Y == y)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }

        static bool CheckCollision(List<Point> snake)
        {
            return snake.Skip(1).Any(s => s.Equals(snake[0]));
        }

        static Direction ChangeDirection(ConsoleKey key, Direction currentDirection, bool isPlayer1)
        {
            if (isPlayer1)
            {
                return key switch
                {
                    ConsoleKey.W when currentDirection != Direction.Down => Direction.Up,
                    ConsoleKey.S when currentDirection != Direction.Up => Direction.Down,
                    ConsoleKey.A when currentDirection != Direction.Right => Direction.Left,
                    ConsoleKey.D when currentDirection != Direction.Left => Direction.Right,
                    _ => currentDirection
                };
            }
            else
            {
                return key switch
                {
                    ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
                    ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
                    ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
                    ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
                    _ => currentDirection
                };
            }
        }
    }

    struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}