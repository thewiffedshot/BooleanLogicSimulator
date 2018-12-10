using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Point = BinaryLogic.Point;

namespace BinaryLogic
{
    public class Arc
    {
        public Point[] ends = new Point[2];
        public Point[] leads = new Point[2];
        
        public Arc(Point topEnd, Point botEnd, Point topLead, Point botLead)
        {
            ends[0] = topEnd;
            ends[1] = botEnd;
            leads[0] = topLead;
            leads[1] = botLead;
        }

        public void Move(Direction direction, uint units = 1)
        {
            switch (direction)
            {
                case Direction.Up:
                    ends[0].Y -= (int)units;
                    ends[1].Y -= (int)units;
                    leads[0].Y -= (int)units;
                    leads[1].Y -= (int)units;
                    break;
                case Direction.Down:
                    ends[0].Y += (int)units;
                    ends[1].Y += (int)units;
                    leads[0].Y += (int)units;
                    leads[1].Y += (int)units;
                    break;
                case Direction.Right:
                    ends[0].X += (int)units;
                    ends[1].X += (int)units;
                    leads[0].X += (int)units;
                    leads[1].X += (int)units;
                    break;
                case Direction.Left:
                    ends[0].X -= (int)units;
                    ends[1].X -= (int)units;
                    leads[0].X -= (int)units;
                    leads[1].X -= (int)units;
                    break;
            }
        }
    }
}
