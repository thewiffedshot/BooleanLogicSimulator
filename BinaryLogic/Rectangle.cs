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
        public int Width { get; set; }
        public int Height { get; set; }

        public Rectangle(Point pos, int width, int height)
        {
            position = pos;
            Width = width;
            Height = height;
            Color = Color.Black;
        }

        public Rectangle(Point pos, int width, int height, Color color)
        {
            position = pos;
            Width = width;
            Height = height;
        }

        public void Move(Direction direction, uint units = 1)
        {
            switch (direction)
            {
                case Direction.Up:
                    position.Y -= (int)units;
                    break;
                case Direction.Down:
                    position.Y += (int)units;
                    break;
                case Direction.Right:
                    position.X += (int)units;
                    break;
                case Direction.Left:
                    position.X -= (int)units;
                    break;
            }
        }

        public void Draw(IRenderer renderer)
        {
            renderer.DrawLine(new Line(position, new Point(position.X + Width, position.Y)), Color, 3);
            renderer.DrawLine(new Line(new Point(position.X + Width, position.Y), new Point(position.X + Width, position.Y + Height)), Color, 3);
            renderer.DrawLine(new Line(position, new Point(position.X, position.Y + Height)), Color, 3);
            renderer.DrawLine(new Line(new Point(position.X + Width, position.Y + Height), new Point(position.X, position.Y + Height)), Color, 3);
        }

        public void ChangeColor(Color color)
        {
            Color = color;
        }
    }
}
