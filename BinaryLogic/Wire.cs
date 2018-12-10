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
        InHitbox inConnected;   // Input hitbox of component were wire ends.
        OutHitbox outConnected; // Output hitbox of component were wire starts.

        public Wire(Scene scene, Line wire, Component input, Component output)
            : base(ComponentType.Wire, null, 3)
        {
            inputs = new List<Component>[1];

            if (input != null)
            {
                inputs[0] = new List<Component>()
                {
                    input
                };
                outConnected = input.outHitbox;
            }

            outputs = new List<Component>(0);

            if (output != null)
                outputs[0] = output;

            lines = new Line[1];
            lines[0] = wire;
            startLine = wire;
        }

        public override List<Component> Transmit(List<Component> outputs, bool signal) // TODO: Breadth first search.
        {
            List<Component> found = new List<Component>(0);

            if (outputs == null) return found;
            
            foreach (Component output in outputs)
            {
                if (output is Wire)
                {
                    found.Concat(((Wire)output).Transmit(found, signal));
                }
                else
                {
                    var result = new List<Component>(0)
                    {
                        output
                    };

                    if (output.outputs != null)
                        output.Set(signal);

                    return result;
                }
            }

            return null;
        }

        public override void Deselect()
        {
            ChangeColor(Color.Black);
        }

        public override void Draw(IRenderer renderer)
        {
            renderer.DrawLine(lines[0], Color, 3);
        }

        public override bool Select(Point location, Scene sender)
        {
            Point higher = lines[0].points.OrderBy(y => y.Y).First();
            Point lower = lines[0].points.OrderBy(y => y.Y).Last();

            Point colinear = new Point(higher.Y - lower.Y,
                                       higher.X - lower.X);

            float parameter = (higher.X - lower.X) / colinear.X;

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

        public override void Scale(Scene scene) // TODO: Deceptively hard.
        {
            if (inputs[0][0] != null)
                lines[0].points[0] = inputs[0][0].outHitbox.Position; // Future implementations will need to include 'inConnected' field for multiple component outputs support.

            if (outputs.Count > 0)
                lines[0].points[1] = outConnected.Position;
            else
            {
                Point delta = scene.ScaleFactor * startLine.points[1] - startLine.points[1];
                lines[0].points[1] = scene.ScaleFactor * startLine.points[1] - delta;
            }
        }
    }
}
