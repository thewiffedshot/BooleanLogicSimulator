using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;

namespace BinaryLogic.Components
{
    internal class XORGate : Component
    {
        public XORGate(Scene scene, Point position)
            : base(ComponentType.XOR, new ComponentHitbox(new Rectangle(position, (int)scene.GetGridInterval() * 2, (int)scene.GetGridInterval() * 2)))
        {
            StartPosition = position / scene.ScaleFactor;
            Position = position;
            Signal = false;

            arcs = new Arc[4];

            Point indent = position + new Point((int)XIndent, (int)YIndent);

            hitbox.Position = Position;

            inHitboxes = new InHitbox[2];
            inHitboxes[0] = new InHitbox(new Point(hitbox.Position.X + (int)XIndent, hitbox.Position.Y + (int)(hitbox.Height / 4)), this, (int)IOHitboxRadius, 0);
            inHitboxes[1] = new InHitbox(new Point(hitbox.Position.X + (int)XIndent, hitbox.Position.Y + (int)(3 * hitbox.Height / 4)), this, (int)IOHitboxRadius, 1);

            outHitbox = new OutHitbox(new Point(hitbox.Position.X + (int)hitbox.Width, hitbox.Position.Y + (int)(hitbox.Height / 2)), this, (int)IOHitboxRadius, 0);

            uint interval = scene.GetGridInterval();

            arcs[0] = new Arc(indent + new Point((int)(interval / 4), 0),
                              indent + new Point((int)(interval / 4), 0) + new Point(0, (int)(interval * 2) - (int)(2 * YIndent)),
                              indent + new Point((int)(interval / 4), 0) + new Point((int)(interval / 2), (int)(interval * 2 / 3f)),
                              indent + new Point((int)(interval / 4), 0) + new Point(0, (int)(interval * 2) - (int)(2 * YIndent))
                                     + new Point((int)(interval / 2), -(int)(interval * 2 / 3f)));

            arcs[1] = new Arc(indent + new Point((int)(interval / 4), 0),
                              indent + new Point((int)interval * 2 - (int)(2 * XIndent), (int)(interval - YIndent)),
                              indent + new Point((int)(interval * 4.5f / 3f), 0),
                              indent + new Point((int)(interval * 5f / 3f), (int)(interval * 1 / 2f)));

            arcs[2] = new Arc(indent + new Point((int)(interval / 4), 0) + new Point(0, (int)(interval * 2) - (int)(2 * YIndent)),

                              indent + new Point(0, (int)(interval * 2) - (int)(2 * YIndent))
                                     + new Point((int)interval * 2 - (int)(2 * XIndent), -(int)(interval - YIndent)),

                              indent + new Point(0, (int)(interval * 2) - (int)(2 * YIndent))
                                     + new Point((int)(interval * 4.5f / 3f), 0),

                              indent + new Point(0, (int)(interval * 2) - (int)(2 * YIndent))
                                     + new Point((int)(interval * 5f / 3f), -(int)(interval * 1 / 2f)));

            arcs[3] = new Arc(indent,
                              indent + new Point(0, (int)(interval * 2) - (int)(2 * YIndent)),
                              indent + new Point((int)(interval / 2), (int)(interval * 2 / 3f)),
                              indent + new Point(0, (int)(interval * 2) - (int)(2 * YIndent))
                                     + new Point((int)(interval / 2), -(int)(interval * 2 / 3f)));
        }

        public override void ChangeColor(Color color)
        {
            Color = color;
        }

        public override void Deselect()
        {
            throw new NotImplementedException();
        }

        public override void Draw(IRenderer renderer)
        {
            foreach (Arc arc in arcs)
                renderer.DrawArc(arc, Color, 3);

            foreach (InHitbox hitbox in inHitboxes)
                hitbox.Draw(renderer);

            outHitbox.Draw(renderer);
        }

        public override void Process(Scene scene)
        {
            bool input1 = false;
            bool input2 = false;

            foreach (Component component in inputs[0])
                if (component.Signal) input1 = true;

            foreach (Component component in inputs[1])
                if (component.Signal) input2 = true;

            Signal = input1 ^ input2;

            foreach (Component component in outputs)
                if (component is Wire)
                    ((Wire)component).Propagate(new List<Wire>(0), this);
        }

        public override void Scale(Scene scene, bool zoom)
        {
            Position = scene.ScaleFactor * StartPosition;
            Position = scene.Grid.SnapToGrid(Position);

            Point indent = Position + scene.ScaleFactor * new Point((int)XIndent, (int)YIndent);

            ChangeColor(Color);

            uint interval = scene.GetGridInterval();

            hitbox.Position = Position;
            hitbox.Width = interval * 2f;
            hitbox.Height = interval * 2f;

            inHitboxes[0].Position = new Point(hitbox.Position.X + (int)XIndent, hitbox.Position.Y + (int)(hitbox.Height / 4));
            inHitboxes[0].Radius = (int)(scene.ScaleFactor * IOHitboxRadius);

            inHitboxes[1].Position = new Point(hitbox.Position.X + (int)XIndent, hitbox.Position.Y + (int)(3 * hitbox.Height / 4));
            inHitboxes[1].Radius = (int)(scene.ScaleFactor * IOHitboxRadius);

            outHitbox.Position = new Point(hitbox.Position.X + (int)hitbox.Width, hitbox.Position.Y + (int)(hitbox.Height / 2));
            outHitbox.Radius = (int)(scene.ScaleFactor * IOHitboxRadius);


            arcs[0] = new Arc(indent + new Point((int)(interval / 4), 0),
                              indent + new Point((int)(interval / 4), 0) + new Point(0, (int)(interval * 2) - (int)(2 * YIndent)),
                              indent + new Point((int)(interval / 4), 0) + new Point((int)(interval / 2), (int)(interval * 2 / 3f)),
                              indent + new Point((int)(interval / 4), 0) + new Point(0, (int)(interval * 2) - (int)(2 * YIndent))
                                     + new Point((int)(interval / 2), -(int)(interval * 2 / 3f)));

            arcs[1] = new Arc(indent + new Point((int)(interval / 4), 0),
                              indent + new Point((int)interval * 2 - (int)(2 * XIndent), (int)(interval - YIndent)),
                              indent + new Point((int)(interval * 4.5f / 3f), 0),
                              indent + new Point((int)(interval * 5f / 3f), (int)(interval * 1 / 2f)));

            arcs[2] = new Arc(indent + new Point((int)(interval / 4), 0) + new Point(0, (int)(interval * 2) - (int)(2 * YIndent)),

                              indent + new Point(0, (int)(interval * 2) - (int)(2 * YIndent))
                                     + new Point((int)interval * 2 - (int)(2 * XIndent), -(int)(interval - YIndent)),

                              indent + new Point(0, (int)(interval * 2) - (int)(2 * YIndent))
                                     + new Point((int)(interval * 4.5f / 3f), 0),

                              indent + new Point(0, (int)(interval * 2) - (int)(2 * YIndent))
                                     + new Point((int)(interval * 5f / 3f), -(int)(interval * 1 / 2f)));

            arcs[3] = new Arc(indent,
                              indent + new Point(0, (int)(interval * 2) - (int)(2 * YIndent)),
                              indent + new Point((int)(interval / 2), (int)(interval * 2 / 3f)),
                              indent + new Point(0, (int)(interval * 2) - (int)(2 * YIndent))
                                     + new Point((int)(interval / 2), -(int)(interval * 2 / 3f)));

            foreach (Component output in outputs)
                if (output is Wire)
                    output.Scale(scene, zoom);
        }

        public override bool Select(Point location, Scene sender)
        {
            bool result = hitbox.Clicked(location);

            InHitbox i = InputClicked(location);
            OutHitbox o = OutputClicked(location);

            if (i != null)
            {
                result = false;
                if (!sender.WirePlacementMode)
                    sender.WireMode(location, this, i);
            }

            else if (o != null)
            {
                result = false;
                if (!sender.WirePlacementMode)
                    sender.WireMode(location, this, o, true);
            }

            return result;
        }

        public override void Translate(Scene scene, Direction direction, uint units = 1)
        {
            switch (direction)
            {
                case Direction.Down:
                    StartPosition.Y += (int)(scene.GetGridInterval() / scene.ScaleFactor * units);
                    break;
                case Direction.Up:
                    StartPosition.Y -= (int)(scene.GetGridInterval() / scene.ScaleFactor * units);
                    break;
                case Direction.Left:
                    StartPosition.X -= (int)(scene.GetGridInterval() / scene.ScaleFactor * units);
                    break;
                case Direction.Right:
                    StartPosition.X += (int)(scene.GetGridInterval() / scene.ScaleFactor * units);
                    break;
            }

            Scale(scene, false);

            foreach (Wire wire in outputs)
                wire.Scale(scene, false);

            foreach (Wire wire in inputs[0])
                wire.Scale(scene, true);

            foreach (Wire wire in inputs[1])
                wire.Scale(scene, true);

            scene.Draw();
        }
    }
}
