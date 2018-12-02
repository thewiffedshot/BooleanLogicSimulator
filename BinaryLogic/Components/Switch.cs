using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;
using Point = BinaryLogic.Point;

namespace BinaryLogic.Components
{
    public class Switch : Component, IClickable
    {
        public Switch(Scene scene, Point position)
            : base(ComponentType.Switch, new ComponentHitbox(new Rectangle(position, 2 * scene.GetGridInterval(), 2 * scene.GetGridInterval())), 3)
        {
            rectangles = new Rectangle[2];
            lines = new Line[1];

            rectangles[0] = new Rectangle(position, 2 * scene.GetGridInterval(), 2 * scene.GetGridInterval());
            rectangles[1] = new Rectangle(position + new Point(10, 5), rectangles[0].Width - 20, rectangles[0].Height - 10);

            lines[0] = new Line(new Point(rectangles[1].position.X, rectangles[1].position.Y + 7), 
                                new Point(rectangles[1].position.X + rectangles[1].Width, rectangles[1].position.Y + 7));
        }

        public void Flip()
        {
            Signal = !Signal;

            if (Signal)
            {
                lines[0].points[0].Y = rectangles[1].position.Y + rectangles[1].Height - 7;
                lines[0].points[1].Y = rectangles[1].position.Y + rectangles[1].Height - 7;
            }
            else
            {
                lines[0].points[0].Y = rectangles[1].position.Y + 7;
                lines[0].points[1].Y = rectangles[1].position.Y + 7;
            }
        }

        public override void ChangeColor(Color color)
        {
            Color = color;

            foreach (Rectangle rectangle in rectangles)
                rectangle.ChangeColor(color);
        }

        public void Click(Point location)
        {
            throw new NotImplementedException();
        }

        public override void Deselect()
        {
            throw new NotImplementedException();
        }

        public override void Draw(IRenderer renderer)
        {
            foreach (Line line in lines)
                renderer.DrawLine(line, Color, Thickness);

            foreach (Rectangle rectangle in rectangles)
                rectangle.Draw(renderer);
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }

        public override bool Select(Point location)
        {
            throw new NotImplementedException();
        }

        public override void Translate(Direction direction, float units)
        {
            throw new NotImplementedException();
        }

        public override List<Component> Transmit(List<Component> outputs, bool signal)
        {
            throw new NotImplementedException();
        }
    }
}
