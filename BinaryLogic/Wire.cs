﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;
using BinaryLogic.Components;

namespace BinaryLogic
{
    internal class Wire : Component
    {
        Line startLine;
        public InHitbox InConnected { get; set; }   // Input hitbox of component where wire ends.
        public OutHitbox OutConnected { get; set; } // Output hitbox of component where wire starts.
        public List<Component> sources = new List<Component>(0);
        Scene Scene = null;
        bool WireChecked { get; set; } = false;

        public Wire(Scene scene, Line wire, Component input, Component output) : // TODO: Need to add two more checks for appropriate missing output or input.
            base(ComponentType.Wire, null, 3)
        {
            inHitboxes = new InHitbox[inputs.Length];
            hitbox = new ComponentHitbox();

            Scene = scene;

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

                OutConnected = scene.WireOutputHitbox;
                InConnected = scene.WireInputHitbox;
                output.inputs[InConnected.AttachedInputIndex].Add(this);

                inHitboxes[0] = new InHitbox(OutConnected.Position, this, (int)(scene.ScaleFactor * 5f), 0);
                outHitbox = new OutHitbox(InConnected.Position, this, (int)(scene.ScaleFactor * 5f), 0);
            }

            if (input != null && output == null)
            {
                inputs[0] = new List<Component>(1)
                {
                    input
                };

                input.outputs.Add(this);
                OutConnected = scene.WireOutputHitbox;

                inHitboxes[0] = new InHitbox(OutConnected.Position, this, (int)(scene.ScaleFactor * 5f), 0);
                outHitbox = new OutHitbox(wire.points[1], this, (int)(scene.ScaleFactor * 5f), 0);
            }

            if (input == null && output != null)
            {
                outputs = new List<Component>(1)
                {
                    output
                };

                InConnected = scene.WireInputHitbox;
                output.inputs[InConnected.AttachedInputIndex].Add(this);

                inHitboxes[0] = new InHitbox(wire.points[0], this, (int)(scene.ScaleFactor * 5f), 0);
                outHitbox = new OutHitbox(InConnected.Position, this, (int)(scene.ScaleFactor * 5f), 0);
            }

            lines = new Line[1];
            lines[0] = wire;
            startLine = new Line((1 / scene.ScaleFactor) * wire.points[0], (1 / scene.ScaleFactor) * wire.points[1]);

            scene.Update();
            Process(scene);
        }

        public override void Deselect()
        {
            ChangeColor(Color.Black);
        }

        public override void Draw(IRenderer renderer)
        {
            renderer.DrawLine(lines[0], Color, 3);

            if (InConnected == null ||
                outputs
                .Where(c => !(c is Wire))
                .LastOrDefault() != null)
            
                if (inHitboxes[0] != null)
            inHitboxes[0].Draw(renderer);

            if (OutConnected != null)
                outHitbox.Draw(renderer);
        }

        public override bool Select(Point location, Scene sender)
        {
            Point segment = lines[0].points[1] - lines[0].points[0];
            Point toStart = lines[0].points[0] - location;
            Point toEnd = lines[0].points[1] - location;

            float segmentLength = Vector.Length(segment);

            float t = -(segment * toStart) / (segmentLength * segmentLength);

            float distance = 999;
            float threshold = 3;

            OutHitbox o = OutputClicked(location);
            InHitbox i = InputClicked(location);

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

            if (sender.WirePlacementMode)
            {
                if (sender.WireInput && i != null)
                {
                    result = false;

                    foreach (Component component in inputs[0])
                    {
                        if (component is Wire && component == sender.WireInputComponent)
                            result = true;
                    }

                    if (!result)
                    {
                        sender.WireOutputComponent = this;
                        sender.WireInputHitbox = i;
                    }

                    result = false;
                }
                else if (!sender.WireInput && o != null)
                {
                    result = false;
                    sender.WireInputComponent = this;
                    sender.WireOutputHitbox = o;
                }
            }
            else if (o != null)
            {
                result = false;
                sender.WireMode(location, this, o, true);
            }

            return result;
        }

        public override void Process(Scene scene)
        {
            bool signal = false;

            foreach (Component source in sources)
                signal = signal || source.Signal;

            Signal = signal;

            if (Color != Color.Orange)
                ChangeColor(Signal ? Color.Red : Color.Black);
        }

        public void Propagate(List<Wire> wiresChecked, Component source, bool remove = false, bool clear = false)  // TODO: Incorporate some sort of lock flag in scene                                                                                                                   
        {                                                                                                          // so it doesn't just delete a component that's being evaluated
            wiresChecked.Add(this);                                                                                // through recursion.

            if (clear == true)
                sources = new List<Component>(0);

            if (remove && sources.Contains(source))
                sources.Remove(source);
            else if (!remove && !sources.Contains(source))
                sources.Add(source);

            lock (inputs[0])
            {
                foreach (Component component in inputs[0])
                    if (component is Wire && !wiresChecked.Contains(component))
                        ((Wire)component).Propagate(wiresChecked, source, remove, clear);
            }

            lock (outputs)
            {
                foreach (Component component in outputs)
                    if (component is Wire && !wiresChecked.Contains(component))
                        ((Wire)component).Propagate(wiresChecked, source, remove, clear);
            }

            if (source is Clock)
            {
                Process(((Clock)source).activeScene);
                Draw(((Clock)source).activeScene.Renderer);
            }

            bool signal = false;

            foreach (Component src in sources)
                signal = signal || src.Signal;

            Signal = signal;

            if (Color != Color.Orange)
                ChangeColor(Signal ? Color.Red : Color.Black);

            Draw(Scene.Renderer);
        }

        public override void Translate(Scene scene, Direction direction, uint units = 1)
        {
            /*if (InConnected != null)
            {
                InConnected.Move(direction, units);

                if (InConnected.Component is Wire && ((Wire)InConnected.Component).OutConnected != null)
                    ((Wire)InConnected.Component).OutConnected.Move(direction, units);
            }

            if (OutConnected != null)
            {
                OutConnected.Move(direction, units);

                if (OutConnected.Component is Wire && ((Wire)OutConnected.Component).InConnected != null)
                    ((Wire)OutConnected.Component).InConnected.Move(direction, units);
            }

            Scale(scene, false);*/
        }

        public override void Scale(Scene scene, bool zoom)  // TODO: Fix branch connecting wires scaling before release???
        {
            Point startPoint = new Point();
            Point endPoint = new Point();

            if (OutConnected != null && InConnected != null)
            {
                startPoint = OutConnected.Position;
                endPoint = InConnected.Position;
            }
            else if (InConnected == null && OutConnected != null)
            {
                startPoint = OutConnected.Position;
                endPoint = startPoint + (scene.ScaleFactor * startLine.Parameter) * startLine.CollinearVector;

                var newPointsOrdered = endPoint.Y > startPoint.Y ?
                                       new { point1 = startPoint, point2 = endPoint } :
                                       new { point1 = endPoint, point2 = startPoint };

                if ((endPoint - startPoint) * (startLine.points[1] - startLine.points[0]) < 0)
                    endPoint = newPointsOrdered.point2 + (newPointsOrdered.point2 - newPointsOrdered.point1);
            }
            else if (InConnected != null && OutConnected == null)
            {
                endPoint = InConnected.Position;
                startPoint = endPoint - (scene.ScaleFactor * startLine.Parameter) * startLine.CollinearVector;

                var newPointsOrdered = endPoint.Y > startPoint.Y ?
                                       new { point1 = startPoint, point2 = endPoint } :
                                       new { point1 = endPoint, point2 = startPoint };

                if ((endPoint - startPoint) * (startLine.points[1] - startLine.points[0]) < 0)
                    startPoint = newPointsOrdered.point2 + (newPointsOrdered.point2 - newPointsOrdered.point1);
            }

            lines[0] = new Line(startPoint, endPoint);

            outHitbox.Position = lines[0].points[1];

            if (inHitboxes[0] != null)
                inHitboxes[0].Position = lines[0].points[0];

            foreach (Component output in outputs)
                output.Scale(scene, zoom);
        }
    }
}