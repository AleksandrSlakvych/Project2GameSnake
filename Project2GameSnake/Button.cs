using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;

namespace OOPGame
{
    class Button : IGameObject
    {
        private int x, y, width, height;
        private string text;

        public Button() { }

        public Button(int x, int y, int w, int h, string txt)
        {
            this.x = x;
            this.y = y;
            width = w;
            height = h;
            text = txt;
        }

        public void Render(ConsoleGraphics graphics)
        {
            graphics.FillRectangle(0xFF00F000, x, y, width, height);
            graphics.DrawString(text, "Arial", 0xFFF0FF00, x, y);
        }

        public void Update(GameEngine engine)
        {
            int mouseX = Input.MouseX;
            int mouseY = Input.MouseY;

            if (x <= mouseX && x + width >= mouseX && y <= mouseY && y + height >= mouseY && Input.IsMouseLeftButtonDown)
                MouseLeftDown?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler MouseLeftDown;
    }
}
