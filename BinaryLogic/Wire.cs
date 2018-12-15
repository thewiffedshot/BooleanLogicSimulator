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

                inHitboxes[0] = new InHitbox(outConnected.Position, (int)(scene.ScaleFactor * 7.5f), 0);
                outHitbox = new OutHitbox(inConnected.Position, (int)(scene.ScaleFactor * 7.5f), 0);
            }

            if (input != null && output == null)
            {
                inputs[0] = new List<Component>(1)
                {
                    input
                };

                input.outputs.Add(this);
                outConnected = scene.WireOutputHitbox;

                inHitboxes[0] = new InHitbox(outConnected.Position, (int)(scene.ScaleFactor * 7.5f), 0);
            }

            if (input == null && output != null)
            {
                outputs = new List<Component>(1)
                {
                    output
                };

                inConnected = scene.WireInputHitbox;
                output.inputs[inConnected.AttachedInputIndex].Add(this);
                
                outHitbox = new OutHitbox(inConnected.Position, (int)(scene.ScaleFactor * 7.5f), 0);
            }

            lines = new Line[1];
            lines[0] = new Line(scene.ScaleFactor * wire.points[0], scene.ScaleFactor * wire.points[1]);
            startLine = wire;
        }

        public override void Deselect()
        {
            ChangeColor(Color.Black);
        }

        public override void Draw(IRenderer renderer)
        {
            renderer.DrawLine(lines[0], Color, 3);

            if (!(inputs[0].Count > 0))
                inHitboxes[0].Draw(renderer);

            if (!(outputs.Count > 0))
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

        public override void Process()
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

        public override void Scale(Scene scene) // TODO: Not absolutely finished, but sufficient.
        {
            Point startPoint = new Point(0, 0);

            if (outConnected != null)
            {
                startPoint = scene.ScaleFactor * outConnected.Position;
            }
            else if (inConnected != null)
            {
                startPoint = scene.ScaleFactor * inConnected.Position;
            }

            Point endPoint = startPoint - scene.ScaleFactor * startLine.Parameter * startLine.CollinearVector;

            lines[0].points[0] = startPoint;
            lines[0].points[1] = endPoint;

            if (inConnected != null)
                outHitbox = new OutHitbox(startPoint, (int)(scene.ScaleFactor * 7.5f), 0);

            if (outConnected != null)
                inHitboxes[0] = new InHitbox(endPoint, (int)(scene.ScaleFactor * 7.5f), inHitboxes[0].AttachedInputIndex);
        }
    }
}
