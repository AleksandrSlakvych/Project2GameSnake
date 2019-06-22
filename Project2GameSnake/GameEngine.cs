using NConsoleGraphics;
using System.Collections.Generic;
using System.Threading;

namespace OOPGame
{

    public class GameEngine
    {

        private ConsoleGraphics graphics;
        public List<IGameObject> objects = new List<IGameObject>();
        private List<IGameObject> tempObjects = new List<IGameObject>();
        private SnakeF SnakeF;

        public GameEngine(ConsoleGraphics graphics)
        {
            this.graphics = graphics;
            SnakeF = new SnakeF(graphics, this);
            AddObject(SnakeF);
        }

        public void ClearScreen()
        {
            objects.Clear();
            tempObjects.Clear();
        }

        public void Restart()
        {
            ClearScreen();
            tempObjects = new List<IGameObject>();
            SnakeF = new SnakeF(graphics, this);
            AddObject(SnakeF);
            Start();
        }

        public void AddObject(IGameObject obj)
        {
            tempObjects.Add(obj);
        }

        public void Start()
        {
            while (true)
            {
                if(GameSettings.GameOver)
                {
                    objects.RemoveAll(o => o is Part);
                    graphics.FillRectangle(0xFFFFFFFF, 0, 0, graphics.ClientWidth, graphics.ClientHeight);
                }
                // Game Loop
                foreach (var obj in objects)
                    obj.Update(this);

                objects.AddRange(tempObjects);
                tempObjects.Clear();

                // clearing screen before painting new frame
                graphics.FillRectangle(0xFFFFFFFF, 0, 0, graphics.ClientWidth, graphics.ClientHeight);
                foreach (var obj in objects)
                    obj.Render(graphics);

                // double buffering technique is used
                graphics.FlipPages();

                Thread.Sleep(70);
            }
        }
    }
}
