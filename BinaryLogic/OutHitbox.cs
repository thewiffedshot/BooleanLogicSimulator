using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;
using Point = BinaryLogic.Point;

namespace BinaryLogic
{
    internal class OutHitbox : IDrawable
    {
        public int AttachedOutputIndex { get; private set; }
        Circle hitbox;
        public Component Component { get; set; }
        public Point Position { get { return hitbox.position; } set { hitbox.position = value; } }
        public int Radius { get { return hitbox.radius; } set { hitbox.radius = value; } }

        public OutHitbox(Point position, Component component, int radius, int outputIndex)
        {
            AttachedOutputIndex = outputIndex;
            hitbox = new Circle(position, radius);
            Component = component;
        }

        public bool Clicked(Point location)
        {
            return Point.Distance(location, hitbox.position) <= hitbox.radius;
        }

        public void ChangeColor(Color color)
        {
            throw new NotImplementedException();
        }

        public void Move(Direction direction, uint units = 1)
        {
            switch (direction)
            {
                case Direction.Up:
                    Position.Y -= (int)units;
                    break;
                case Direction.Down:
                    Position.Y += (int)units;
                    break;
                case Direction.Right:
                    Position.X += (int)units;
                    break;
                case Direction.Left:
                    Position.X -= (int)units;
                    break;
            }
        }

        public void Draw(IRenderer renderer)
        {
            renderer.DrawCircle(hitbox, Color.Green, 3);
        }
    }
}
