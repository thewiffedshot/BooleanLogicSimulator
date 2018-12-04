using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = BinaryLogic.Point;

namespace BinaryLogic.Interfaces
{
    public interface IClickable
    {
        void Click(Point location);
    }
}
