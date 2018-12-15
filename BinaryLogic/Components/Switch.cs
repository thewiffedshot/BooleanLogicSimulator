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
            : base(ComponentType.Switch, new ComponentHitbox(new Rectangle(position, (int)(2 * scene.GetGridInterval()), (int)(2 * scene.GetGridInterval()))), 3)
        {
            StartPosition = position / scene.ScaleFactor;
            Position = position;
            Signal = false;

            rectangles = new Rectangle[2];
            lines = new Line[1];

            hitbox.Position = Position;
            Point indent = position + new Point((int)XIndent, (int)YIndent);


            rectangles[0] = new Rectangle(indent, (int)(2 * scene.GetGridInterval() - 2 * XIndent * scene.ScaleFactor),
                                                  (int)(2 * scene.GetGridInterval() - 2 * YIndent * scene.ScaleFactor));

            rectangles[1] = new Rectangle(indent + new Point((int)(10 * scene.ScaleFactor), (int)(5 * scene.ScaleFactor)),
                                                                (int)(rectangles[0].Width - 20 * scene.ScaleFactor),
                                                                (int)(rectangles[0].Height - 10 * scene.ScaleFactor));

            outHitbox = new OutHitbox(new Point(position.X + rectangles[0].Width, position.Y + rectangles[0].Height / 2), (int)(scene.ScaleFactor * 7.5f), 0);

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
                lines[0].points[0].Y = (int)(rectangles[1].position.Y + 3 * (float)rectangles[1].Height / 4); // Constants must be explicitly noted.
                lines[0].points[1].Y = (int)(rectangles[1].position.Y + 3 * (float)rectangles[1].Height / 4);
            }
            else
            {
                lines[0].points[0].Y = (int)(rectangles[1].position.Y + (float)rectangles[1].Height / 4);
                lines[0].points[1].Y = (int)(rectangles[1].position.Y + (float)rectangles[1].Height / 4);
            }
        }
        
        public override void ChangeColor(Color color)
        {
            Color = color;

            foreach (Rectangle rectangle in rectangles)
                rectangle.ChangeColor(color);
        }

        public void Click(Point location, Scene sender)
        {
            if (hitbox.Clicked(location))
                Flip();
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

            outHitbox.Draw(renderer);
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }

        public override bool Select(Point location, Scene sender)
        {
            bool result = hitbox.Clicked(location);

            InHitbox i = InputClicked(location);
            OutHitbox o = OutputClicked(location);

            if (i != null)
            {
                result = false;
                sender.WireMode(location, this, i);
            }

            else if (o != null)
            {
                result = false;
                sender.WireMode(location, this, o, true);
            }

            return result;
        }

        public override void Translate(Scene scene, Direction direction, uint units = 1)
        {
            switch (direction)
            {
                case Direction.Down:
                    Position.Y += (int)(scene.GetGridInterval() * units);
                    break;
                case Direction.Up:
                    Position.Y -= (int)(scene.GetGridInterval() * units);
                    break;
                case Direction.Left:
                    Position.X -= (int)(scene.GetGridInterval() * units);
                    break;
                case Direction.Right:
                    Position.X += (int)(scene.GetGridInterval() * units);
                    break;
            }

            Scale(scene);
        }

        public override void Scale(Scene scene)
        {
            Position = scene.ScaleFactor * StartPosition;

            Position = scene.Grid.SnapToGrid(Position);

            Point indent = Position + scene.ScaleFactor * new Point((int)XIndent, (int)YIndent);

            rectangles[0] = new Rectangle(indent, (int)(2 * scene.GetGridInterval() - 2 * XIndent * scene.ScaleFactor),
                                                  (int)(2 * scene.GetGridInterval() - 2 * YIndent * scene.ScaleFactor));

            rectangles[1] = new Rectangle(indent + new Point((int)(10 * scene.ScaleFactor), (int)(5 * scene.ScaleFactor)),
                                                                (int)(rectangles[0].Width - 20 * scene.ScaleFactor),
                                                                (int)(rectangles[0].Height - 10 * scene.ScaleFactor));

            ChangeColor(Color);

            outHitbox = new OutHitbox(new Point(Position.X + rectangles[0].Width, Position.Y + rectangles[0].Height / 2), (int)(scene.ScaleFactor * 7.5f), 0);
            hitbox.Position = Position;

            float yLine = 0;

            if (Signal)
                yLine = 3 * rectangles[1].Height / 4;
            else
                yLine = rectangles[1].Height / 4;

            lines[0] = new Line(new Point(rectangles[1].position.X,
                                          (int)(rectangles[1].position.Y + yLine)),
                                new Point(rectangles[1].position.X + rectangles[1].Width,
                                          (int)(rectangles[1].position.Y + yLine)));
        }
    }
}
