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
            rectangles[1] = new Rectangle(position + new Point(15, 10), rectangles[0].Width - 30, rectangles[0].Height - 20);

            lines[0] = new Line(new Point(rectangles[1].position.X, rectangles[1].position.Y + 15), 
                                new Point(rectangles[1].position.X + rectangles[1].Width, rectangles[1].position.Y + 15));
        }

        public void Flip()
        {
            Signal = !Signal;

            if (Signal)
            {
                lines[0].points[0].Y = rectangles[1].position.Y + rectangles[1].Height - 15;
                lines[0].points[1].Y = rectangles[1].position.Y + rectangles[1].Height - 15;
            }
            else
            {
                lines[0].points[0].Y = rectangles[1].position.Y + 15;
                lines[0].points[1].Y = rectangles[1].position.Y + 15;
            }
        }

        public override void ChangeColor(Color color)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
