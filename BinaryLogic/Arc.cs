using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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

        public void Move(Direction direction, int units)
        {
            switch (direction)
            {
                case Direction.Up:
                    ends[0].Y -= units;
                    ends[1].Y -= units;
                    leads[0].Y -= units;
                    leads[1].Y -= units;
                    break;
                case Direction.Down:
                    ends[0].Y += units;
                    ends[1].Y += units;
                    leads[0].Y += units;
                    leads[1].Y += units;
                    break;
                case Direction.Right:
                    ends[0].X += units;
                    ends[1].X += units;
                    leads[0].X += units;
                    leads[1].X += units;
                    break;
                case Direction.Left:
                    ends[0].X -= units;
                    ends[1].X -= units;
                    leads[0].X -= units;
                    leads[1].X -= units;
                    break;
            }
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    ends[0].Y--;
                    ends[1].Y--;
                    leads[0].Y--;
                    leads[1].Y--;
                    break;
                case Direction.Down:
                    ends[0].Y++;
                    ends[1].Y++;
                    leads[0].Y++;
                    leads[1].Y++;
                    break;
                case Direction.Right:
                    ends[0].X++;
                    ends[1].X++;
                    leads[0].X++;
                    leads[1].X++;
                    break;
                case Direction.Left:
                    ends[0].X--;
                    ends[1].X--;
                    leads[0].X--;
                    leads[1].X--;
                    break;
            }
        }
    }
}
