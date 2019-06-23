using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;

namespace OOPGame
{
    class Part : IGameObject
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int SpeedX { get; set; }
        public int SpeedY { get; set; }
        public uint Colour { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Part(int x, int y, uint colour) // Size of the part of the snake
        {
            X = x;
            Y = y;
            Width = 16;
            Height = 16;
            SpeedX = 0;
            SpeedY = 0;
            Colour = colour;
        }

        public Part() // Size of the square to pick
        {
            X = 0;
            Y = 0;
            Width = 16;
            Height = 16;
            SpeedX = 0;
            SpeedY = 0;
            Colour = 0xFFFFFFFF;
        }

        public void Render(ConsoleGraphics graphics)
        {
            graphics.FillRectangle(Colour, X * Width, Y * Height, Width, Height);
        }

        public void Update(GameEngine engine)
        {

        }
    }
}
