﻿using System;
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
            hitbox = new Circle(position, radius);
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
            renderer.DrawCircle(hitbox, Color.Green, 3);
        }
    }
}
