using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BinaryLogic.Interfaces
{
    public interface IGraphics
    {
        void DrawArc(Arc arc);
        void DrawLine(Arc line);
        void DrawCircle(Circle circle);
    }
}
