using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Point = BinaryLogic.Point;
using BinaryLogic.Interfaces;

namespace BinaryLogic
{
    public class Rectangle : IDrawable
    {
        public Point position;
        public Color Color { get; private set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Rectangle(Point pos, float width, float height)
        {
            position = pos;
            Width = width;
            Height = height;
            Color = Color.Black;
        }

        public Rectangle(Point pos, float width, float height, Color color)
        {
            position = pos;
            Width = width;
            Height = height;
        }

        public void Move(Direction direction, float units = 1)
        {
            switch (direction)
            {
                case Direction.Up:
                    position.Y -= units;
                    break;
                case Direction.Down:
                    position.Y += units;
                    break;
                case Direction.Right:
                    position.X += units;
                    break;
                case Direction.Left:
                    position.X -= units;
                    break;
            }
        }

        public void Draw(IRenderer renderer)
        {
            renderer.DrawLine(new Line(position, new Point(position.X + Width, position.Y)), Color, 3);
            renderer.DrawLine(new Line(new Point(position.X + Width, position.Y), new Point(position.X + Width, position.Y + Height)), Color, 3);
            renderer.DrawLine(new Line(new Point(position.X, position.Y + Height), position), Color, 3);
            renderer.DrawLine(new Line(position, new Point(position.X, position.Y + Height)), Color, 3);
        }

        public void ChangeColor(Color color)
        {
            Color = color;
        }
    }
}
