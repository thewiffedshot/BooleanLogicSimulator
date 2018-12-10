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
    public class OutHitbox : IDrawable
    {
        Circle hitbox;
        public Point Position { get { return hitbox.position; } set { hitbox.position = value; } }

        public OutHitbox(Point position, int radius)
        {
            hitbox = new Circle(position, radius);
        }

        public bool Clicked(Point location)
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
