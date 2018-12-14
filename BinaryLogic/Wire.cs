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
        int length = 0;
        InHitbox inConnected;   // Input hitbox of component where wire ends.
        OutHitbox outConnected; // Output hitbox of component where wire starts.

        public Wire(Scene scene, Line wire, Component input, Component output)  // TODO: Rewrite the checks properly using
                                                                                //       'WireInputComponent' and 'WireOutputComponent' from the scene.
            : base(ComponentType.Wire, null, 3)
        {  
            if (input != null)
            {
                inputs[0] = new List<Component>(1)
                {
                    input
                };

                outConnected = input.outHitbox;
                inHitboxes = new InHitbox[inputs.Length];
                //inHitboxes[0] = new InHitbox(outConnected.Position, (int)(scene.ScaleFactor * 7.5f));
            }

            outputs = new List<Component>(0);

            if (output != null)
            {
                outputs = new List<Component>(1)
                {
                    output
                };

                inConnected = output.inHitboxes.Where(h => h.AttachedComponent == this).SingleOrDefault();
                //outHitbox = new OutHitbox(inConnected.Position, (int)(scene.ScaleFactor * 7.5f));
            }

            lines = new Line[1];
            lines[0] = wire;
            startLine = wire;

            length = (int)Point.Distance(startLine.points[0], startLine.points[1]);
        }

        public override void Deselect()
        {
            ChangeColor(Color.Black);
        }

        public override void Draw(IRenderer renderer)
        {
            renderer.DrawLine(lines[0], Color, 3);

            if (inputs[0].Count > 0)
                inHitboxes[0].Draw(renderer);

            if (outputs.Count > 0)
                outHitbox.Draw(renderer);
        }

        public override bool Select(Point location, Scene sender)
        {
            Point higher = lines[0].points.OrderBy(y => y.Y).First();
            Point lower = lines[0].points.OrderBy(y => y.Y).Last();

            Vector colinear = lines[0].CollinearVector;

            float parameter = lines[0].Parameter;

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

            Point endPoint = startPoint + scene.ScaleFactor * startLine.Parameter * startLine.CollinearVector;

            lines[0].points[0] = startPoint;
            lines[0].points[1] = endPoint;

            if (inConnected != null)
                outHitbox = new OutHitbox(startPoint, (int)(scene.ScaleFactor * 7.5f));

            if (outConnected != null)
                inHitboxes[0] = new InHitbox(endPoint, (int)(scene.ScaleFactor * 7.5f));
        }
    }
}
