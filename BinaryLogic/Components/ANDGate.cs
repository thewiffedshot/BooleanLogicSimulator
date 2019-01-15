using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;

namespace BinaryLogic.Components
{
    internal class ANDGate : Component
    {
        public ANDGate(Scene scene, Point position) 
            : base(ComponentType.AND, new ComponentHitbox(new Rectangle(position, (int)scene.GetGridInterval() * 2, (int)scene.GetGridInterval() * 2)))
        {
            StartPosition = position / scene.ScaleFactor;
            Position = position;
            Signal = false;

            lines = new Line[3];
            arcs = new Arc[1];

            Point indent = position + new Point((int)XIndent, (int)YIndent);

            hitbox.Position = Position;

            inHitboxes = new InHitbox[2];
            inHitboxes[0] = new InHitbox(new Point(hitbox.Position.X + (int)XIndent, hitbox.Position.Y + (int)(hitbox.Height / 4)), this, (int)IOHitboxRadius, 0);
            inHitboxes[1] = new InHitbox(new Point(hitbox.Position.X + (int)XIndent, hitbox.Position.Y + (int)(3 * hitbox.Height / 4)), this, (int)IOHitboxRadius, 1);

            outHitbox = new OutHitbox(new Point(hitbox.Position.X + (int)hitbox.Width, hitbox.Position.Y + (int)(hitbox.Height / 2)), this, (int)IOHitboxRadius, 0);

            uint interval = scene.GetGridInterval();

            lines[0] = new Line(indent, 
                                new Point(indent.X, indent.Y + (int)(interval * 2 - (2 * YIndent * scene.ScaleFactor))));

            lines[1] = new Line(indent, 
                                new Point(indent.X + (int)(interval * 4f / 3), indent.Y));

            lines[2] = new Line(lines[0].points[1],
                                new Point(indent.X + (int)(interval * 4f / 3), indent.Y + (int)(interval * 2 - (2 * YIndent * scene.ScaleFactor))));

            arcs[0] = new Arc(lines[1].points[1],
                              lines[2].points[1],
                              lines[1].points[1] + new Point((int)(interval / 1.5f), (int)(interval / 4f)),
                              lines[2].points[1] + new Point((int)(interval / 1.5f), -(int)(interval / 4f)));
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
            foreach (Line line in lines)
                renderer.DrawLine(line, Color, Thickness);

            foreach (Arc arc in arcs)
                renderer.DrawArc(arc, Color, Thickness);

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

            Signal = input1 && input2;

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


            lines[0] = new Line(indent, 
                                new Point(indent.X, indent.Y + (int)((interval * 2) - (2 * YIndent * scene.ScaleFactor))));

            lines[1] = new Line(indent, 
                                new Point(indent.X + (int)(interval * 4f / 3), indent.Y));

            lines[2] = new Line(lines[0].points[1],
                                new Point(indent.X + (int)(interval * 4f / 3), indent.Y + (int)(interval * 2 - (2 * YIndent * scene.ScaleFactor))));

            arcs[0] = new Arc(lines[1].points[1],
                              lines[2].points[1],
                              lines[1].points[1] + new Point((int)(interval / 1.5f), (int)(interval / 4f)),
                              lines[2].points[1] + new Point((int)(interval / 1.5f), -(int)(interval / 4f)));

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
