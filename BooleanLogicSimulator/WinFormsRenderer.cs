using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BinaryLogic;
using BinaryLogic.Interfaces;
using Point = BinaryLogic.Point;

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

        public void ClearArea(BinaryLogic.Rectangle area, Color color)
        {
            using (Pen pen = new Pen(color))
            {
                gfx.DrawRectangle(pen, area.position.X, area.position.Y, area.Width, area.Height);
            }
        }

        public void DrawArc(Arc arc, Color color, uint thickness)
        {
            throw new NotImplementedException();
        }

        public void DrawCircle(Circle circle, Color color, uint thickness)
        {
            using (Pen pen = new Pen(color, thickness))
            {
                SolidBrush brush = new SolidBrush(color);      

                //gfx.DrawEllipse(pen, circle.position.X, circle.position.Y, circle.radius, circle.radius);
                gfx.FillEllipse(brush, circle.position.X, circle.position.Y, circle.radius, circle.radius);
            }
        }

        public void DrawLine(Line line, Color color, uint thickness)
        {
            using (Pen pen = new Pen(color, thickness))
                gfx.DrawLine(pen, line.points[0].X, line.points[0].Y, line.points[1].X, line.points[1].Y);
        }
    }
}
