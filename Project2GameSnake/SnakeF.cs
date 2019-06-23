using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;

namespace OOPGame
{
    public enum Direction { Up, Down, Left, Right };

    class SnakeF : IGameObject
    {
        private int fieldWidth, fieldHeight;
        private List<Part> snake;
        private Part head;
        private Part food;
        private Button buttonRestart, buttonHighScore;
        private string message;
        private Random random = new Random();
        private GameEngine engine;

        private Direction Direction { get; set; }
        public bool GameOver { get; set; }
        private bool GameInfo { get; set; }
        private int Score { get; set; }
        private int Points { get; set; }

        public SnakeF(ConsoleGraphics graphics, GameEngine engine)
        {
            this.engine = engine;
            snake = new List<Part>();
            head = new Part(20, 20, 0xFFFF0000);
            snake.Add(head);
            fieldHeight = graphics.ClientHeight;
            fieldWidth = graphics.ClientWidth;
            Direction = Direction.Down;
            GameOver = false;
            GameInfo = false;
            Score = 0;
            Points = 100;
            food = new Part();
            food.X = random.Next(0, fieldWidth / food.Width);
            food.Colour = 0xFFFFFF00;
            food.Y = random.Next(0, fieldHeight / food.Height);
            food.Render(graphics);
            buttonRestart = new Button(10, 10, 200, 50, "NEW GAME");
            buttonHighScore = new Button(10, 120, 200, 50, "BEST SCORE");
            engine.ClearScreen();
            engine.AddObject(head);
            engine.AddObject(food);
        }

        public void Render(ConsoleGraphics graphics)
        {
            if (!GameOver)
            {
                for (int i = 0; i < snake.Count; i++)
                {
                    graphics.FillRectangle(snake[i].Colour, snake[i].X * snake[i].Width, snake[i].Y * head.Height, head.Width, head.Height);
                }
            }
            else if (GameInfo)
            {
                ResumeGame(graphics);
            }
            else
            {
                message = "GAME OVER. Your Result: " + Score.ToString() + " points";
                ResumeGame(graphics);
            }
        }

        public void Update(GameEngine engine)
        {
            buttonHighScore.Update(engine);
            buttonRestart.Update(engine);

            if (!GameOver)
            {
                if (Input.IsKeyDown(Keys.RIGHT) && Direction != Direction.Left)
                    Direction = Direction.Right;
                else if (Input.IsKeyDown(Keys.LEFT) && Direction != Direction.Right)
                    Direction = Direction.Left;
                else if (Input.IsKeyDown(Keys.UP) && Direction != Direction.Down)
                    Direction = Direction.Up;
                else if (Input.IsKeyDown(Keys.DOWN) && Direction != Direction.Up)
                    Direction = Direction.Down;

                MovePlayer(engine);
            }
        }

        private void CreateFood(GameEngine engine)
        {
            bool needNewFood = true;

            while (needNewFood)
            {
                food = new Part();
                food.X = random.Next(0, fieldWidth / food.Width);
                food.Colour = 0xFFFFFF00;
                food.Y = random.Next(0, fieldHeight / food.Height);

                for (int i = 0; i < snake.Count; i++)
                    if (!(snake[i].X == food.X && snake[i].Y == food.Y))
                        needNewFood = false;
            }
            engine.AddObject(food);
        }

        private void Die(GameEngine engine)
        {
            GameOver = true;
        }

        private void Eat(GameEngine engine)
        {
            food.Colour = 0xFF00FF00;
            food.X = snake[snake.Count - 1].X;
            food.Y = snake[snake.Count - 1].Y;
            snake.Add(food);
            CreateFood(engine);
            Score += Points;
        }

        private void MovePlayer(GameEngine engine)
        {
            for (int i = snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Direction)
                    {
                        case Direction.Right:
                            snake[i].SpeedX = 1;
                            snake[i].SpeedY = 0;
                            break;
                        case Direction.Left:
                            snake[i].SpeedX = -1;
                            snake[i].SpeedY = 0;
                            break;
                        case Direction.Down:
                            snake[i].SpeedX = 0;
                            snake[i].SpeedY = 1;
                            break;
                        case Direction.Up:
                            snake[i].SpeedX = 0;
                            snake[i].SpeedY = -1;
                            break;
                    }

                    snake[i].X += snake[i].SpeedX;
                    snake[i].Y += snake[i].SpeedY;

                    int maxXPos = fieldWidth / head.Width;
                    int maxYPos = fieldHeight / head.Height;

                    if (snake[0].X < 0 || snake[0].Y < 0 || snake[0].X >= maxXPos || snake[0].Y >= maxYPos)
                    {
                        Die(engine);
                    }

                    for (int j = 1; j < snake.Count; j++)
                    {
                        if (snake[i].X == snake[j].X && snake[i].Y == snake[j].Y)
                        {
                            Die(engine);
                        }
                    }

                    if (snake[0].X == food.X && snake[0].Y == food.Y)
                    {
                        Eat(engine);
                    }
                }

                else
                {
                    snake[i].X = snake[i - 1].X;
                    snake[i].Y = snake[i - 1].Y;
                }
            }
        }

        private void ResumeGame(ConsoleGraphics graphics)
        {
            graphics.DrawString(message, "Arial", 0xFFFF0000, 100, 300);
            buttonRestart.Render(graphics);
            buttonHighScore.Render(graphics);
            buttonRestart.MouseLeftDown += MouseRestart;
            buttonHighScore.MouseLeftDown += HighScore;
            UpdatePoints(Score);
        }

        private void HighScore(object sender, EventArgs e)
        {
            string path = "Points.txt";
            string score = File.ReadAllText(path);
            message = "BEST RESULT : " + score;
            GameOver = true;
            GameInfo = true;
        }

        private void MouseRestart(object obj, EventArgs e)
        {
            engine.Restart();
            GameInfo = false;
        }

        private void UpdatePoints(int points)
        {
            int hightScore = 0;
            string path = "Points.txt";
            if (File.Exists(path))
            {
                string str = File.ReadAllText(path);
                hightScore = int.Parse(str);
            }

            if (points >= hightScore)
                File.WriteAllText(path, points.ToString());
        }
    }
}
