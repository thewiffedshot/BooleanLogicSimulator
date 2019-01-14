using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;

namespace BinaryLogic.Components
{
    internal class Clock : Component, IClickable
    {
        double tFactor = 1;

        Timer timer;
        public Scene activeScene;

        public Clock(Scene scene, Point position)
            : base(ComponentType.Clock, new ComponentHitbox(new Rectangle(position, (int)scene.GetGridInterval() * 2, (int)scene.GetGridInterval() * 2)))
        {
            StartPosition = position / scene.ScaleFactor;
            Position = position;
            Signal = false;

            activeScene = scene;

            circles = new Circle[1];

            Point indent = position + new Point((int)XIndent, (int)YIndent);

            hitbox.Position = Position;

            outHitbox = new OutHitbox(new Point(hitbox.Position.X + (int)hitbox.Width, hitbox.Position.Y + (int)(hitbox.Height / 2)), (int)IOHitboxRadius, 0);

            uint interval = scene.GetGridInterval();
            int radius = (int)(interval - XIndent);

            circles[0] = new Circle(indent + new Point(radius, radius),
                                    radius);

            timer = new Timer(tInterval * tFactor)
            {
                AutoReset = true
            };
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Start();
        }

        public override void Deselect()
        {
            throw new NotImplementedException();
        }

        public override void Draw(IRenderer renderer)
        {
            foreach (Circle circle in circles)
                renderer.DrawCircle(circle, Color, Thickness, false);

            outHitbox.Draw(renderer);
        }

        public override void ChangeColor(Color color)
        {
            Color = color;
        }

        public override void Process(Scene scene)
        {
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

            outHitbox.Position = new Point(hitbox.Position.X + (int)hitbox.Width, hitbox.Position.Y + (int)(hitbox.Height / 2));
            outHitbox.Radius = (int)(scene.ScaleFactor * IOHitboxRadius);

            int radius = (int)(interval - XIndent);

            circles[0] = new Circle(indent + new Point(radius, radius),
                                    radius);
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
                    Scale(scene, false);
                    break;
                case Direction.Up:
                    StartPosition.Y -= (int)(scene.GetGridInterval() / scene.ScaleFactor * units);
                    Scale(scene, false);
                    break;
                case Direction.Left:
                    StartPosition.X -= (int)(scene.GetGridInterval() / scene.ScaleFactor * units);
                    Scale(scene, false);
                    break;
                case Direction.Right:
                    StartPosition.X += (int)(scene.GetGridInterval() / scene.ScaleFactor * units);
                    Scale(scene, false);
                    break;
            }

            foreach (Wire wire in outputs)
                wire.Scale(scene, true);

            scene.Draw();
        }

        public void Click(Point location, Scene sender)
        {
            tFactor += 1;
            tFactor %= 5;
            tFactor += tFactor == 0 ? 1 : 0;

            timer.Interval = tInterval * tFactor;
        }

        async void OnTimedEvent(Object sender, ElapsedEventArgs e)
        {
            Signal = !Signal;

            foreach (Component component in outputs)
                if (component is Wire)
                    ((Wire)component).Propagate(new List<Wire>(0), this);
        }
    }
}
