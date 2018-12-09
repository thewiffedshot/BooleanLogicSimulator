using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;
using BinaryLogic.Components;
using Point = BinaryLogic.Point;
using System.Timers; // TODO: Must incorporate settable tickrate.

namespace BinaryLogic
{
    public enum Key { Up, Down, Left, Right, Shift, Control, Space, Q, W, E, R, T, Y, Plus, Minus, Invalid }
    public enum MouseKey { Left, Right, Middle, Invalid }

    public class Scene 
    {
        IRenderer renderer;
        public Grid Grid { get; set; }
        public float ScaleFactor { get; private set; }
        public Point Offset { get; set; }
        public Color Background { get; set; }
        public List<Component> components = new List<Component>(0);
        public Component SelectedComponent { get; private set; }

        public bool WirePlacementMode { get; private set; }
        public Point WireStart { get; private set; }
        public Point MouseLocation { get; private set; }
        public bool WireInput { get; private set; }
        public Component WireInputComponent { get; private set; }
        public Component WireOutputComponent { get; private set; }

        public uint GlobalID { get; set; }

        public Scene(Grid grid, Color background, IRenderer renderer)
        {
            Grid = grid;
            Background = background;
            this.renderer = renderer;
            ScaleFactor = 1;
            Offset = new Point(0, 0);
        }

        public void SetRenderer(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public void AddComponent(Component component)
        {
            component.ID = GlobalID++;

            components.Add(component);
            Draw();
        }

        public void SelectComponent(Point location)
        {
            Component selected = components
                                 .Where(c => c.Select(location, this))
                                 .OrderBy(c => c.ID)
                                 .LastOrDefault();

            if (SelectedComponent == selected)
                DeselectComponent(selected);
            else
            {
                DeselectComponent(SelectedComponent);
                SelectedComponent = selected;

                if (SelectedComponent != null)
                    SelectedComponent.ChangeColor(Color.Orange);
            }

            Draw(true);
        }

        public void DeselectComponent(Component component)
        {
            if (SelectedComponent != null)
            {
                component.ChangeColor(Component.DefaultColor);
                SelectedComponent = null;
                component.ChangeColor(Component.DefaultColor);
            }
        }

        public void Draw(bool componentsOnly = false)
        {
            if (WirePlacementMode)
            {
                if (!componentsOnly)
                    Clear();

                renderer.DrawLine(new Line(WireStart, MouseLocation), Color.Black, 3);

                foreach (Component component in components)
                {
                    component.Draw(renderer);
                }
            }
            else
            {
                if (!componentsOnly)
                    Clear();

                foreach (Component component in components)
                {
                    component.Draw(renderer);
                }
            }
        }

        public void Clear()
        {
            renderer.Clear(Background);
            Grid.Draw(renderer);
        }

        public void WireMode(Point start, Component sender, bool input = false)
        {
            WirePlacementMode = true;
            WireInput = input;
            WireStart = start;

            if (WireInput)
                WireInputComponent = sender;
            else
                WireOutputComponent = sender;
        }

        public void MouseMove(Point location)
        {
            MouseLocation = location;

            if (WirePlacementMode)
            {
                Draw();
            }
        }

        public void MouseClick(MouseKey key, Point location)
        {
            switch (key)
            {
                case MouseKey.Left:
                    if (WirePlacementMode)
                    {
                        if (WireInput)
                        {
                            WireOutputComponent = components.Where(c => c.InputClicked(location) != null)
                                                            .OrderBy(c => c.ID)
                                                            .LastOrDefault();
                        }
                        else
                        {
                            WireInputComponent = components.Where(c => c.OutputClicked(location) != null)
                                                            .OrderBy(c => c.ID)
                                                            .LastOrDefault();
                        }

                        AddComponent(new Wire(this, new Line(WireStart, location), WireInputComponent, WireOutputComponent));
                        WirePlacementMode = false;
                        WireStart = null;
                        WireInputComponent = null;
                        WireOutputComponent = null;
                    }
                    else
                    {
                        SelectComponent(location);
                        Draw();
                    }
                    break;
                case MouseKey.Right:
                    if (WirePlacementMode)
                    {
                        WirePlacementMode = false;
                        WireStart = null;
                        WireInputComponent = null;
                        WireOutputComponent = null;
                    }
                    else
                    { 
                        foreach (IClickable c in components)
                            c.Click(location);

                        Draw();
                    }
                    break;
                default:
                    if (WirePlacementMode)
                    {
                        WirePlacementMode = false;
                        WireStart = null;
                        WireInputComponent = null;
                        WireOutputComponent = null;
                    }
                    throw new NotImplementedException();
            }
        }

        public void KeyStroke(Key key, Point mouseLocation)
        {
            Point closest = Grid.SnapToGrid(mouseLocation);
            
            if (!WirePlacementMode)
            switch (key)
            {
                case Key.T:
                    AddComponent(new Switch(this, closest));
                    break;
                case Key.Plus:
                    Scale(0.25f);
                    break;
                case Key.Minus:
                    Scale(-0.25f);
                    break;
            }
        }

        public float GetGridInterval()
        {
            return Grid.Interval;
        }

        public void Scale(float scale)
        {
            if (ScaleFactor + scale > 0)
            {
                ScaleFactor += scale;
                Grid.Scale(scale);

                foreach (Component component in components)
                {
                    component.Scale(this);
                }

                Draw();
            }
        }
    }
}
