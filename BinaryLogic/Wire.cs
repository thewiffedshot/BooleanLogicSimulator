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
                outHitbox = new OutHitbox(wire.points[1], (int)(scene.ScaleFactor * 5f), 0);
            }

            if (input == null && output != null)
            {
                outputs = new List<Component>(1)
                {
                    output
                };

                inConnected = scene.WireInputHitbox;
                output.inputs[inConnected.AttachedInputIndex].Add(this);

                inHitboxes[0] = new InHitbox(wire.points[0], (int)(scene.ScaleFactor * 5f), 0);
                outHitbox = new OutHitbox(inConnected.Position, (int)(scene.ScaleFactor * 5f), 0);
            }

            lines = new Line[1];
            lines[0] = wire;
            startLine = new Line((1 / scene.ScaleFactor) * wire.points[0], (1 / scene.ScaleFactor) * wire.points[1]);

            Process(scene);
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

        public override bool Select(Point location, Scene sender)      // TODO: Need to fix wire branch creation. 
        {                                                              // Make sure IO hitboxes are properly checked when clicked
            Point segment = lines[0].points[1] - lines[0].points[0];
            Point toStart = lines[0].points[0] - location;
            Point toEnd = lines[0].points[1] - location;

            float segmentLength = Vector.Length(segment);

            float t = -(segment * toStart) / (segmentLength * segmentLength);

            float distance = 999;
            float threshold = 3;

            InHitbox i = InputClicked(location);
            OutHitbox o = OutputClicked(location);

            bool result = false;

            if (t > 0 && t < 1)
            {
                float TS = Vector.Length(toStart);
                float SEdotTS = segment * toStart;

                distance = TS * TS - ((SEdotTS * SEdotTS) / (segmentLength * segmentLength));

                if (distance <= threshold * threshold)
                {
                    result = true;
                }
            }

            if (o != null)
            {
                result = false;
                sender.WireMode(location, this, o, true);
            }

            return result;
        }

        public override void Process(Scene scene)
        {
            Signal = false;

            foreach (Component component in inputs[0])
            {
                if (component.Signal)
                    Signal = true;
            }

            ChangeColor(Signal ? Color.Red : Color.Black);
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

            outHitbox.Position *= scene.ScaleFactor;
            inHitboxes[0].Position *= scene.ScaleFactor;
        }
    }
}
