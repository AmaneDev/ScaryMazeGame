using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ScaryMaze.GameLogic
{
    public class Player
    {
        public Rectangle UI { get; private set; }

        public double X { get; private set; }
        public double Y { get; private set; }

        public Player(Rectangle rect)
        {
            UI = rect;
        }

        public void Move(double x, double y)
        {
            X = x - UI.Width / 2;
            Y = y - UI.Height / 2;

            Canvas.SetLeft(UI, X);
            Canvas.SetTop(UI, Y);
        }
    }
}