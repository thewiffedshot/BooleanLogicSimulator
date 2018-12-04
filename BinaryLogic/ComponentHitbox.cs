using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using BinaryLogic.Interfaces;
using Point = BinaryLogic.Point;

namespace BinaryLogic
{
    public class ComponentHitbox : IDrawable
    {
        Rectangle hitbox;
        public Point Position { get { return hitbox.position; } set { hitbox.position = value; } }
        public float Width { get { return hitbox.Width; } }
        public float Height { get { return hitbox.Height; } }

        public ComponentHitbox(Rectangle rectangle)
        {
            hitbox = rectangle;
        }

        public void ChangeColor(Color color)
        {
            throw new NotImplementedException();
        }

        public bool Clicked(Point location)
        {
            return location.X > hitbox.position.X &&
                   location.X < hitbox.position.X + hitbox.Width &&
                   location.Y > hitbox.position.Y &&
                   location.Y < hitbox.position.Y + hitbox.Height;
        }

        public void Draw(IRenderer renderer)
        {
            hitbox.Draw(renderer);
        }

        public void Translate(Direction direction, float units = 1)
        {
            hitbox.Move(direction, units);
        }
    }
}
