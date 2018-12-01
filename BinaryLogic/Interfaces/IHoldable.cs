using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = BinaryLogic.Point;

namespace BinaryLogic.Interfaces
{
    public interface IHoldable
    {
        void Pressed(Point location);
        void Released();
    }
}
