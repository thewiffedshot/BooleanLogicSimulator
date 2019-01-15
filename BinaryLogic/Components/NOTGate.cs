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
        Circle startCircle = new Circle(new Point(), 0);

        public NOTGate(Scene scene, Point position)
            : base(ComponentType.NOT, new ComponentHitbox(new Rectangle(position, (int) scene.GetGridInterval() * 2, (int) scene.GetGridInterval() * 2)))
        {   
            StartPosition = position / scene.ScaleFactor;
            Position = position;
            Signal = false;
            
            lines = new Line[3];
            circles = new Circle[1];
            
            Point indent = position + new Point((int)XIndent, (int)YIndent);
            
            hitbox.Position = Position;
               
            inHitboxes = new InHitbox[1];
            inHitboxes[0] = new InHitbox(new Point(hitbox.Position.X + (int)XIndent, hitbox.Position.Y + (int)(hitbox.Height / 2)), this, (int)IOHitboxRadius, 0);
            
            outHitbox = new OutHitbox(new Point(hitbox.Position.X + (int)hitbox.Width, hitbox.Position.Y + (int)(hitbox.Height / 2)), this, (int)IOHitboxRadius, 0);

            uint interval = scene.GetGridInterval();

            lines[0] = new Line(indent,
                                new Point(indent.X, indent.Y + (int)(interval * 2 - (2 * YIndent * scene.ScaleFactor))));
            
            lines[1] = new Line(indent,
                                new Point(indent.X + (int)(interval * 2f - 4 * XIndent), indent.Y + (int)(interval - YIndent)));

            lines[2] = new Line(lines[0].points[1],
                                new Point(indent.X + (int)(interval * 2f - 4 * XIndent), indent.Y + (int)(interval - YIndent)));

            circles[0] = new Circle(new Point(indent.X + (int)(interval * 2f - 3 * XIndent), indent.Y + (int)(interval - YIndent)), (int)(XIndent));

            startCircle = circles[0];
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
                renderer.DrawLine(line, Color, 3);

            foreach (Circle circle in circles)
                renderer.DrawCircle(circle, Color, 3, false);

            if (inHitboxes[0] != null)
                inHitboxes[0].Draw(renderer);

            if (outHitbox != null)
                outHitbox.Draw(renderer);
        }

        public override void Process(Scene scene)
        {
            bool input = false;

            foreach (Component component in inputs[0])
                if (component.Signal) input = true;

            Signal = !input;

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

            inHitboxes[0].Position = new Point(hitbox.Position.X + (int)XIndent, hitbox.Position.Y + (int)(hitbox.Height / 2));
            inHitboxes[0].Radius = (int)(scene.ScaleFactor * IOHitboxRadius);

            outHitbox.Position = new Point(hitbox.Position.X + (int)hitbox.Width, hitbox.Position.Y + (int)(hitbox.Height / 2));
            outHitbox.Radius = (int)(scene.ScaleFactor * IOHitboxRadius);


            lines[0] = new Line(indent,
                                new Point(indent.X, indent.Y + (int)(interval * 2 - (2 * YIndent * scene.ScaleFactor))));

            lines[1] = new Line(indent,
                                new Point(indent.X + (int)(interval * 2f - 4 * XIndent * scene.ScaleFactor), indent.Y + (int)(interval - YIndent * scene.ScaleFactor)));

            lines[2] = new Line(lines[0].points[1],
                                new Point(indent.X + (int)(interval * 2f - 4 * XIndent * scene.ScaleFactor), indent.Y + (int)(interval - YIndent * scene.ScaleFactor)));

            circles[0] = new Circle(new Point(indent.X + (int)(interval * 2f - 3 * XIndent * scene.ScaleFactor), indent.Y + (int)(interval - YIndent * scene.ScaleFactor)), startCircle.radius * (int)(scene.ScaleFactor));

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

            scene.Draw();
        }
    }
}
