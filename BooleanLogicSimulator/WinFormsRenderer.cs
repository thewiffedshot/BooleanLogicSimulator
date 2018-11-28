using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BinaryLogic;
using BinaryLogic.Interfaces;

namespace BooleanLogicSimulator
{
    class WinFormsRenderer : IRenderer
    {
        Graphics gfx;

        public WinFormsRenderer(Graphics gfx)
        {
            this.gfx = gfx;
        }

        public void Clear(Color clearColor)
        {
            gfx.Clear(clearColor);
        }

        public void DrawArc(Arc arc, Color color, uint thickness)
        {
            throw new NotImplementedException();
        }

        public void DrawCircle(Circle circle, Color color, uint thickness)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(Line line, Color color, uint thickness)
        {
            using (Pen pen = new Pen(color, thickness))
                gfx.DrawLine(pen, line.points[0].X, line.points[0].Y, line.points[1].X, line.points[1].Y);
        }
    }
}
