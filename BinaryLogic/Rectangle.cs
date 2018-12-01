using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BinaryLogic
{
    public class Rectangle
    {
        public Point position;
        public uint Width { get; set; }
        public uint Height { get; set; }

        public Rectangle(Point pos, uint width, uint height)
        {
            position = pos;
            Width = width;
            Height = height;
        }

        public void Move(Direction direction, int units = 1)
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
    }
}
