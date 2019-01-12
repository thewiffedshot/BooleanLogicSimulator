using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;

namespace BinaryLogic.Components
{
    internal class ORGate : Component
    {
        public ORGate(Scene scene, Point position) 
            : base(ComponentType.AND, new ComponentHitbox(new Rectangle(position, (int)scene.GetGridInterval() * 2, (int)scene.GetGridInterval() * 2)), 3)
        {
            StartPosition = position / scene.ScaleFactor;
            Position = position;
            Signal = false;

            arcs = new Arc[3];

            Point indent = position + new Point((int)XIndent, (int)YIndent);

            hitbox.Position = Position;

            inHitboxes = new InHitbox[2];
            inHitboxes[0] = new InHitbox(new Point(hitbox.Position.X + (int)XIndent, hitbox.Position.Y + (int)(hitbox.Height / 4)), 5, 0);
            inHitboxes[1] = new InHitbox(new Point(hitbox.Position.X + (int)XIndent, hitbox.Position.Y + (int)(3 * hitbox.Height / 4)), 5, 1);

            outHitbox = new OutHitbox(new Point(hitbox.Position.X + (int)hitbox.Width, hitbox.Position.Y + (int)(hitbox.Height / 2)), 5, 0);

            arcs[0] = new Arc(indent,
                              indent + new Point(0, (int)(scene.GetGridInterval() * 2) - (int)(2 * YIndent)),
                              indent + new Point((int)(scene.GetGridInterval() / 3 * 2), (int)(scene.GetGridInterval() / 2)),
                              indent + new Point((int)(scene.GetGridInterval() / 3), (int)(scene.GetGridInterval() / 2)));


        }

        public override void ChangeColor(Color color)
        {
            throw new NotImplementedException();
        }

        public override void Deselect()
        {
            throw new NotImplementedException();
        }

        public override void Draw(IRenderer renderer)
        {
            //foreach (Arc arc in arcs)
                renderer.DrawArc(arcs[0], Color, 3);
        }

        public override void Process(Scene scene)
        {
            throw new NotImplementedException();
        }

        public override void Scale(Scene scene, bool zoom)
        {
            throw new NotImplementedException();
        }

        public override bool Select(Point location, Scene sender)
        {
            throw new NotImplementedException();
        }

        public override void Translate(Scene scene, Direction direction, uint units = 1)
        {
            throw new NotImplementedException();
        }
    }
}
