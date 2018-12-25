using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;
using BinaryLogic;

namespace BinaryLogic
{
    class Wire : Component
    {
        Line startLine;
        InHitbox inConnected;   // Input hitbox of component where wire ends.
        OutHitbox outConnected; // Output hitbox of component where wire starts.

        public Wire(Scene scene, Line wire, Component input, Component output) : // TODO: Need to add two more checks for appropriate missing output or input.
            base(ComponentType.Wire, null, 3)
        {
            inHitboxes = new InHitbox[inputs.Length];
            hitbox = new ComponentHitbox();

            if (input != null && output != null)
            {
                inputs[0] = new List<Component>(1)
                {
                    input
                };

                input.outputs.Add(this);

                outputs = new List<Component>(1)
                {
                    output
                };

                outConnected = scene.WireOutputHitbox;
                inConnected = scene.WireInputHitbox;
                output.inputs[inConnected.AttachedInputIndex].Add(this);

                inHitboxes[0] = new InHitbox(outConnected.Position, (int)(scene.ScaleFactor * 5f), 0);
                outHitbox = new OutHitbox(inConnected.Position, (int)(scene.ScaleFactor * 5f), 0);
            }

            if (input != null && output == null)
            {
                inputs[0] = new List<Component>(1)
                {
                    input
                };

                input.outputs.Add(this);
                outConnected = scene.WireOutputHitbox;

                inHitboxes[0] = new InHitbox(outConnected.Position, (int)(scene.ScaleFactor * 5f), 0);
            }

            if (input == null && output != null)
            {
                outputs = new List<Component>(1)
                {
                    output
                };

                inConnected = scene.WireInputHitbox;
                output.inputs[inConnected.AttachedInputIndex].Add(this);
                
                outHitbox = new OutHitbox(inConnected.Position, (int)(scene.ScaleFactor * 5f), 0);
            }

            lines = new Line[1];
            lines[0] = wire;
            startLine = new Line((1 / scene.ScaleFactor) * wire.points[0], (1 / scene.ScaleFactor) * wire.points[1]);
        }

        public override void Deselect()
        {
            ChangeColor(Color.Black);
        }

        public override void Draw(IRenderer renderer)
        {
            renderer.DrawLine(lines[0], Color, 3);

            if (inConnected == null)
                inHitboxes[0].Draw(renderer);

            if (outConnected == null)
                outHitbox.Draw(renderer);
        }

        public override bool Select(Point location, Scene sender)
        {
            Point higher = startLine.points.OrderBy(y => y.Y).First();
            Point lower = startLine.points.OrderBy(y => y.Y).Last();

            Vector colinear = startLine.CollinearVector;

            float parameter = startLine.Parameter;

            float lineX = colinear.X * parameter + higher.X;
            float lineY = colinear.Y * parameter + higher.Y;

            return Math.Abs(lineX - location.X) <= Thickness &&
                   Math.Abs(lineY - location.Y) <= Thickness &&
                   location.Y > lower.Y && location.Y < higher.Y;
        }

        public override void Process(Scene scene)
        {
            Signal = false;

            foreach (Component component in inputs[0])
            {
                if (component.Signal == true)
                    Signal = true;
            }
        }

        public override void Translate(Scene scene, Direction direction, uint units = 1)
        {
            throw new NotImplementedException();
        }

        public override void Scale(Scene scene, bool zoom)
        {
            Point startPoint = new Point();
            Point endPoint = new Point();

            if (outConnected != null && inConnected != null)
            {
                startPoint = outConnected.Position;
                endPoint = inConnected.Position;
            }
            else if (inConnected == null && outConnected != null)
            {
                startPoint = outConnected.Position;
                endPoint = startPoint + scene.ScaleFactor * startLine.Parameter * startLine.CollinearVector;
            }
            else if (inConnected != null && outConnected == null)
            {
                endPoint = inConnected.Position;
                startPoint = endPoint + scene.ScaleFactor * startLine.Parameter * startLine.CollinearVector;
            }
            else
                throw new InvalidOperationException("Wire can not exist without at least one attached IOhitbox.");

            lines[0] = new Line(startPoint, endPoint);

            outHitbox = new OutHitbox(startPoint, (int)(scene.ScaleFactor * 5f), 0);

            if (inHitboxes[0] != null)
                inHitboxes[0] = new InHitbox(endPoint, (int)(scene.ScaleFactor * 5f), inHitboxes[0].AttachedInputIndex);
        }
    }
}
