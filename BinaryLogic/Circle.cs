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
        public uint radius;

        public Circle(Point pos, uint radius)
        {
            position = pos;
            this.radius = radius;
        }

        public void Move(Direction direction, int units)
        {
            switch (direction)
            {
                case Direction.Up:
                    position.Y -= units;
                    break;
                case Direction.Down:
                    position.Y += units;
                    break;
                case Direction.Right:
                    position.X += units;
                    break;
                case Direction.Left:
                    position.X -= units;
                    break;
            }
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    position.Y--;
                    break;
                case Direction.Down:
                    position.Y++;
                    break;
                case Direction.Right:
                    position.X++;
                    break;
                case Direction.Left:
                    position.X--;
                    break;
            }
        }
    }
}
