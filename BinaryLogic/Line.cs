using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Point = BinaryLogic.Point;

namespace BinaryLogic
{
    public class Line
    {
        public Point[] points = new Point[2];

        public Line(Point point1, Point point2)
        {
            points = new Point[]{ point1, point2 };
        }

        public void Move(Direction direction, uint units = 1)
        {
            switch (direction)
            {
                case Direction.Up:
                    points[0].Y -= (int)units;
                    points[1].Y -= (int)units;
                    break;
                case Direction.Down:
                    points[0].Y += (int)units;
                    points[1].Y += (int)units;
                    break;
                case Direction.Right:
                    points[0].X += (int)units;
                    points[1].X += (int)units;
                    break;
                case Direction.Left:
                    points[0].X -= (int)units;
                    points[1].X -= (int)units;
                    break;
            }
        }
    }
}
