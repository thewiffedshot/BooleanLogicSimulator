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
        Form form;

        public WinFormsRenderer(Form form)
        {
            this.form = form;
        }

        public void Clear(Color clearColor)
        {
            Graphics gfx = form.CreateGraphics();

            gfx.Clear(clearColor);
        }

        public void ClearArea(BinaryLogic.Rectangle area, Color color)
        {
            Graphics gfx = form.CreateGraphics();

            using (Pen pen = new Pen(color))
            {
                gfx.DrawRectangle(pen, area.position.X, area.position.Y, area.Width, area.Height);
            }
        }

        public void DrawArc(Arc arc, Color color, uint thickness)
        {
            Graphics gfx = form.CreateGraphics();

            using (Pen pen = new Pen(color, thickness))
            {
                gfx.DrawBezier(pen, arc.ends[0].X, arc.ends[0].Y, arc.leads[0].X, arc.leads[0].Y, arc.leads[1].X, arc.leads[1].Y, arc.ends[1].X, arc.ends[1].Y);
            }
        }

        public void DrawCircle(Circle circle, Color color, uint thickness, bool fill)
        {
            Graphics gfx = form.CreateGraphics();

            if (fill)
                using (SolidBrush brush = new SolidBrush(color)) 
                    gfx.FillEllipse(brush, circle.position.X - circle.radius, circle.position.Y - circle.radius, 2 * circle.radius, 2 * circle.radius);
            else
                using (Pen pen = new Pen(color, thickness))
                    gfx.DrawEllipse(pen, circle.position.X - circle.radius, circle.position.Y - circle.radius, 2 * circle.radius, 2 * circle.radius);
        }

        public void DrawLine(Line line, Color color, uint thickness)
        {
            Graphics gfx = form.CreateGraphics();

            using (Pen pen = new Pen(color, thickness))
                lock (this)
                {
                    gfx.DrawLine(pen, line.points[0].X, line.points[0].Y, line.points[1].X, line.points[1].Y);
                }
        }
    }
}
