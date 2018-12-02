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
        public Wire(Scene scene, Line wire)
            : base(ComponentType.Wire, null, 3)
        {
            lines = new Line[1];
            lines[0] = wire;
        }

        public override List<Component> Transmit(List<Component> outputs, bool signal)
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
                    var result = new List<Component>();
                    result.Add(output);

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

        public override bool Select(Point location)
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
    }
}
