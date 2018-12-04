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
        void DrawArc(Arc arc, Color color, uint thickness);
        void DrawLine(Line line, Color color, uint thickness);
        void DrawCircle(Circle circle, Color color, uint thickness);
        void Clear(Color clearColor);
        void ClearArea(Rectangle area, Color clearColor);
    }
}
