using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using BinaryLogic.Interfaces;

namespace BinaryLogic
{
    public class ComponentHitbox : IDrawable, IClickable
    {
        Rectangle hitbox;

        public ComponentHitbox(Rectangle rectangle)
        {
            hitbox = rectangle;
        }

        public void ChangeColor(Color color)
        {
            throw new NotImplementedException();
        }

        public bool Click(Point location)
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

        public void SetPostition(Point position)
        {
            hitbox.position = position;
        }
    }
}
