using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfTheHill.ColorCraze
{
    public class ColorCrazeDecision : Decision
    {
        private Point _movement;

        public Point Movement
        {
            get
            {
                return _movement;
            }
            set
            {
                var absX = Math.Abs(value.X);
                var absY = Math.Abs(value.Y);
                var totalDistance = absX + absY;
                if (totalDistance > 1 || absX > 1 || absY > 1)
                {
                    _movement = new Point(0, 0);
                }
                else
                {
                    _movement = value;
                }
            }
        }

        public ColorCrazeDecision(ColorCrazeDirection dir)
        {
            switch (dir)
            {
                case ColorCrazeDirection.Up:
                    Movement = new Point(0, -1);
                    break;
                case ColorCrazeDirection.Down:
                    Movement = new Point(0, 1);
                    break;
                case ColorCrazeDirection.Left:
                    Movement = new Point(-1, 0);
                    break;
                case ColorCrazeDirection.Right:
                    Movement = new Point(1, 0);
                    break;
                default:
                    Movement = new Point(0, 0);
                    break;
            }
        }

        public ColorCrazeDecision(Point dir)
        {
            Movement = dir;
        }
    }

    public enum ColorCrazeDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}
