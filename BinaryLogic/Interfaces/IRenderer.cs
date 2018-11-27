using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BinaryLogic.Interfaces
{
    public interface IRenderer
    {
        void DrawArc(Arc arc);
        void DrawLine(Arc line);
        void DrawCircle(Circle circle);
        void Clear(Color clearColor);
    }
}
