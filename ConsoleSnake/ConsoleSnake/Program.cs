﻿namespace ConsoleSnake
{
    class Program
    {
        static void Main(string[] args)
        {
            var snake1 = new List<Point> { new Point(10, 5) };
            var food = GetFood(snake1, 50, 20);
            var direction1 = Direction.Right;
            var boardWidth = 50;
            var boardHeight = 20;
            var score1 = 0; // Inicjalizacja wyniku poza pętlą główną
            var random = new Random();

            Console.CursorVisible = false;
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    direction1 = ChangeDirection(key, direction1, true);
                }

                MoveSnake(snake1, direction1);

                if (snake1[0].X == food.X && snake1[0].Y == food.Y)
                {
                    snake1.Add(new Point(snake1[^1].X, snake1[^1].Y));
                    food = GetNewFoodPosition(snake1, boardWidth, boardHeight, random);
                    score1++; // Zwiększenie wyniku po zdobyciu jedzenia
                }

                if (CheckCollision(snake1, boardWidth, boardHeight))
                {
                    snake1 = new List<Point> { new Point(10, 5) };
                    food = GetFood(snake1, boardWidth, boardHeight);
                    direction1 = Direction.Right;
                    // score1 = 0; // Nie ustawiamy wyniku na 0 po kolizji
                }

                Console.Clear();
                DrawBoard(snake1, food, boardWidth, boardHeight);
                Console.WriteLine($"Score1: {score1}"); // Wyświetlenie aktualnego wyniku
                Thread.Sleep(200);
            }
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

        static Point GetNewFoodPosition(List<Point> snake, int width, int height, Random random)
        {
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

        static void DrawBoard(List<Point> snake, Point food, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (snake.Any(s => s.X == x && s.Y == y))
                    {
                        Console.Write("*");
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

        static bool CheckCollision(List<Point> snake, int width, int height)
        {
            return snake.Skip(1).Any(s => s.Equals(snake[0])) || snake[0].X < 0 || snake[0].Y < 0 || snake[0].X >= width || snake[0].Y >= height;
        }

        static Direction ChangeDirection(ConsoleKey key, Direction currentDirection, bool isPlayer1)
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
