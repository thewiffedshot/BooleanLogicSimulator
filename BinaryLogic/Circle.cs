using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Point = BinaryLogic.Point;

namespace BinaryLogic
{
    public class Circle
    {
        public Point position;
        public int radius;

        public Circle(Point pos, int radius)
        {
            position = pos;
            this.radius = radius;
        }

        public void Move(Direction direction, uint units = 1)
        {
            switch (direction)
            {
                case Direction.Up:
                    position.Y -= (int)units;
                    break;
                case Direction.Down:
                    position.Y += (int)units;
                    break;
                case Direction.Right:
                    position.X += (int)units;
                    break;
                case Direction.Left:
                    position.X -= (int)units;
                    break;
            }
        }
    }
}
