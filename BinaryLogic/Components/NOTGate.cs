using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;

namespace BinaryLogic.Components
{
    internal class NOTGate : Component
    {
        public NOTGate(Scene scene, Point position)
            : base(ComponentType.NOT, new ComponentHitbox(new Rectangle(position, (int) scene.GetGridInterval() * 2, (int) scene.GetGridInterval() * 2)), 3)
        {
            StartPosition = position / scene.ScaleFactor;
            Position = position;
            Signal = false;

            lines = new Line[3];
            circles = new Circle[1];

            Point indent = position + new Point((int)XIndent, (int)YIndent);

            hitbox.Position = Position;

            inHitboxes = new InHitbox[1];
            inHitboxes[0] = new InHitbox(new Point(hitbox.Position.X + (int)XIndent, hitbox.Position.Y + (int)(hitbox.Height / 2)), 5, 0);

            outHitbox = new OutHitbox(new Point(hitbox.Position.X + (int)hitbox.Width, hitbox.Position.Y + (int)(hitbox.Height / 2)), 5, 0);

            lines[0] = new Line(indent,
                                new Point(indent.X, indent.Y + (int)(scene.GetGridInterval() * 2 - (2 * YIndent * scene.ScaleFactor))));

            lines[1] = new Line(indent,
                                new Point(indent.X + (int)(scene.GetGridInterval() * 2f - 2 * XIndent), indent.Y + (int)(scene.GetGridInterval() - YIndent)));

            lines[2] = new Line(lines[0].points[1],
                                new Point(indent.X + (int)(scene.GetGridInterval() * 2f - 2 * XIndent), indent.Y + (int)(scene.GetGridInterval() - YIndent)));
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
            foreach (Line line in lines)
                renderer.DrawLine(line, Color, 3);
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
