using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = BinaryLogic.Point;

namespace BinaryLogic
{
    public class PointField
    {
        public Point[,] points;

        public PointField(Grid grid)
        {
            if (grid == null)
                throw new ArgumentNullException("Grid not defined.");

            points = new Point[(int)(grid.WindowSize.X / grid.Interval), (int)(grid.WindowSize.Y / grid.Interval)];

            for (uint y = 0, j = 0; y < points.GetLength(1); y++, j += grid.Interval)
            {
                for (uint x = 0, i = 0; x < points.GetLength(0); x++, i += grid.Interval)
                {
                    points[x,y] = new Point(i, j);
                }
            }
        }
    }
}
