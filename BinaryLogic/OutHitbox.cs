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
    public class OutHitbox : IDrawable, IClickable
    {
        Circle hitbox;

        public OutHitbox(Point position, float radius)
        {
            hitbox = new Circle(hitbox.position = position,
                                hitbox.radius = radius);
        }

        public bool Click(Point location)
        {
            return Point.Distance(location, hitbox.position) <= hitbox.radius;
        }

        public void ChangeColor(Color color)
        {
            throw new NotImplementedException();
        }

        public void Draw(IRenderer renderer)
        {
            throw new NotImplementedException();
        }
    }
}
