﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;

namespace BinaryLogic.Components
{
    public class Light : Component
    {
        Circle startCircle;

        public Light(Scene scene, Point position) : base(ComponentType.Light, new ComponentHitbox(new Rectangle(position, (int)(2 * scene.GetGridInterval()), (int)(2 * scene.GetGridInterval()))), 3)
        {
            StartPosition = position / scene.ScaleFactor;
            Position = position;
            Signal = false;

            rectangles = new Rectangle[1];
            circles = new Circle[1];

            hitbox.Position = Position;
            Point indent = position + new Point((int)XIndent, (int)YIndent);


            rectangles[0] = new Rectangle(indent, (int)(2 * scene.GetGridInterval() - 2 * XIndent * scene.ScaleFactor),
                                                  (int)(2 * scene.GetGridInterval() - 2 * YIndent * scene.ScaleFactor));

            circles[0] = new Circle(new Point(rectangles[0].position.X + rectangles[0].Width / 2,
                                              rectangles[0].position.Y + rectangles[0].Height / 2), 
                                              (int)((4f / 10) * rectangles[0].Width));

            startCircle = circles[0];

            inHitboxes = new InHitbox[1];
            inHitboxes[0] = new InHitbox(new Point(rectangles[0].position.X, rectangles[0].position.Y + rectangles[0].Height / 2), (int)(scene.ScaleFactor * 5f), 0);
        }

        public override void ChangeColor(Color color)
        {
            Color = color;

            foreach (Rectangle rectangle in rectangles)
                rectangle.ChangeColor(color);
        }

        public override void Deselect()
        {
            throw new NotImplementedException();
        }

        public override void Draw(IRenderer renderer)
        {
            rectangles[0].Draw(renderer);

            renderer.DrawCircle(circles[0], Signal ? Color.LightGreen : Color.Red, 3);

            inHitboxes[0].Draw(renderer);
        }

        public override void Process()
        {
            bool result = false;

            if (inputs[0].Count > 0)
            {
                foreach (Component input in inputs[0])
                {
                    if (input.Signal) result = true;
                }
            }

            Signal = result;
        }

        public override void Scale(Scene scene, bool zoom)
        {
            Position = scene.ScaleFactor * StartPosition;

            Position = scene.Grid.SnapToGrid(Position);

            Point indent = Position + scene.ScaleFactor * new Point((int)XIndent, (int)YIndent);

            rectangles[0] = new Rectangle(indent, (int)(2 * scene.GetGridInterval() - 2 * XIndent * scene.ScaleFactor),
                                                    (int)(2 * scene.GetGridInterval() - 2 * YIndent * scene.ScaleFactor));

            circles[0] = new Circle(new Point(rectangles[0].position.X + rectangles[0].Width / 2,
                                              rectangles[0].position.Y + rectangles[0].Height / 2),
                                    (int)(scene.ScaleFactor * startCircle.radius));

            ChangeColor(Color);

            inHitboxes[0].Position = new Point(rectangles[0].position.X, rectangles[0].position.Y + rectangles[0].Height / 2);
            inHitboxes[0].Radius = (int)(scene.ScaleFactor * 5f);
            hitbox.Position = Position;
            // TODO: Scale hitbox accordingly.
        }

        public override bool Select(Point location, Scene sender)
        {
            bool result = hitbox.Clicked(location);

            InHitbox i = InputClicked(location);
            OutHitbox o = OutputClicked(location);

            if (i != null)
            {
                result = false;
                sender.WireMode(location, this, i);
            }

            else if (o != null)
            {
                result = false;
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

            scene.Draw();
        }
    }
}
