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
            Position = position;
            rectangles = new Rectangle[2];
            lines = new Line[1];

            Point indent = scene.ScaleFactor * position + new Point(XIndent, YIndent);

            rectangles[0] = new Rectangle(indent, 2 * scene.GetGridInterval() - 2 * XIndent * scene.ScaleFactor, 
                                                  2 * scene.GetGridInterval() - 2 * YIndent * scene.ScaleFactor);

            rectangles[1] = new Rectangle(indent + new Point(10 * scene.ScaleFactor, 5 * scene.ScaleFactor), 
                                                                rectangles[0].Width - 20 * scene.ScaleFactor, 
                                                                rectangles[0].Height - 10 * scene.ScaleFactor);

            lines[0] = new Line(new Point(rectangles[1].position.X, 
                                          rectangles[1].position.Y + rectangles[1].Height / 4), 
                                new Point(rectangles[1].position.X + rectangles[1].Width, 
                                          rectangles[1].position.Y + rectangles[1].Height / 4));
        }

        public void Flip()
        {
            Signal = !Signal;

            if (Signal)
            {
                lines[0].points[0].Y = rectangles[1].position.Y + rectangles[1].Height / 4;
                lines[0].points[1].Y = rectangles[1].position.Y + rectangles[1].Height / 4;
            }
            else
            {
                lines[0].points[0].Y = rectangles[1].position.Y + 3 * rectangles[1].Height / 4;
                lines[0].points[1].Y = rectangles[1].position.Y + 3 * rectangles[1].Height / 4;
            }
        }

        public override void ChangeColor(Color color)
        {
            Color = color;

            foreach (Rectangle rectangle in rectangles)
                rectangle.ChangeColor(color);
        }

        public bool Click(Point location)
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

        public override void Translate(Scene scene, Direction direction, uint units = 1)
        {
            switch (direction)
            {
                case Direction.Down:
                    Position.Y += scene.GetGridInterval() * units;
                    break;
                case Direction.Up:
                    Position.Y -= scene.GetGridInterval() * units;
                    break;
                case Direction.Left:
                    Position.X -= scene.GetGridInterval() * units;
                    break;
                case Direction.Right:
                    Position.X += scene.GetGridInterval() * units;
                    break;
            }

            Scale(scene);
        }

        public override List<Component> Transmit(List<Component> outputs, bool signal)
        {
            throw new NotImplementedException();
        }

        public override void Scale(Scene scene)
        {
            Point indent = scene.ScaleFactor * (Position + new Point(XIndent, YIndent));

            rectangles[0] = new Rectangle(indent, 2 * scene.GetGridInterval() - 2 * XIndent * scene.ScaleFactor,
                                                  2 * scene.GetGridInterval() - 2 * YIndent * scene.ScaleFactor);

            rectangles[1] = new Rectangle(indent + new Point(10 * scene.ScaleFactor, 5 * scene.ScaleFactor),
                                                                rectangles[0].Width - 20 * scene.ScaleFactor,
                                                                rectangles[0].Height - 10 * scene.ScaleFactor);

            lines[0] = new Line(new Point(rectangles[1].position.X,
                                          rectangles[1].position.Y + rectangles[1].Height / 4),
                                new Point(rectangles[1].position.X + rectangles[1].Width,
                                          rectangles[1].position.Y + rectangles[1].Height / 4));
        }
    }
}
