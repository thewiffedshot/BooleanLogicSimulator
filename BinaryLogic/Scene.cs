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
    public enum Key { Up, Down, Left, Right, Shift, Control, Space, Q, W, E, R, T, Y, L, Plus, Minus, Invalid }
    public enum MouseKey { Left, Right, Middle, Invalid }

    public class Scene 
    {
        public IRenderer Renderer { get; private set; }
        public Grid Grid { get; set; }
        public float ScaleFactor { get; private set; }
        private float LastScaleFactor { get; set; }
        public Point Offset { get; set; }
        public Color Background { get; set; }
        public List<Component> components = new List<Component>(0);
        public Component SelectedComponent { get; private set; }

        public bool WirePlacementMode { get; private set; }
        public bool WireInput { get; private set; }
        public Point WireStart { get; private set; }
        public Point MouseLocation { get; private set; }
        private Component WireInputComponent { get; set; }
        private Component WireOutputComponent { get; set; }
        public InHitbox WireInputHitbox { get; private set; }
        public OutHitbox WireOutputHitbox { get; private set; }

        public uint GlobalID { get; set; }

        public Scene(Grid grid, Color background, IRenderer renderer)
        {
            Grid = grid;
            Background = background;
            Renderer = renderer;
            ScaleFactor = 1;
            LastScaleFactor = 1;
            Offset = new Point(0, 0);
        }

        public void Update()
        {
            foreach (Component component in components)
            {
                if (component is Switch)
                {
                    component.SetLevel(0);
                }

                else if (component is Button)
                {
                    component.SetLevel(0);
                }
            }

            uint maxLevel = components
                            .OrderBy(c => c.Level)
                            .Last().Level;

            var ordered =   components
                            .OrderBy(c => c.Level)
                            .ToList();

            foreach (Component component in ordered)
            {
                if (component.Level != 0)
                    component.Process(this);
            }

            foreach (Component component in components)
                component.Checked = false;
        }

        public void SetRenderer(IRenderer renderer)
        {
            Renderer = renderer;
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

                Renderer.DrawLine(new Line(WireStart, MouseLocation), Color.Black, 3);

                foreach (Component component in components)
                {
                    component.Draw(Renderer);
                }
            }
            else
            {
                if (!componentsOnly)
                    Clear();

                foreach (Component component in components)
                {
                    component.Draw(Renderer);
                }
            }
        }

        public void Clear()
        {
            Renderer.Clear(Background);
            Grid.Draw(Renderer);
        }

        public void WireMode(Point start, Component sender, object hitbox, bool input = false)
        {
            WirePlacementMode = true;
            WireStart = start;
            WireInput = input;

            if (input)
            {
                WireInputComponent = sender;
                WireOutputHitbox = (OutHitbox)hitbox;
            }
            else
            {
                WireOutputComponent = sender;
                WireInputHitbox = (InHitbox)hitbox;
            }
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
                            WireOutputComponent = components
                                                  .Where(c => c.InputClicked(location) != null)
                                                  .LastOrDefault();

                            if (WireOutputComponent != null)
                            foreach (InHitbox hitbox in WireOutputComponent.inHitboxes)
                            {
                                if (hitbox.Clicked(location))
                                {
                                    WireInputHitbox = hitbox;
                                }
                            }
                        }
                        else
                        {
                            WireInputComponent = components
                                                 .Where(c => c.OutputClicked(location) != null)
                                                 .LastOrDefault();

                            if (WireInputComponent != null)
                                WireOutputHitbox = WireInputComponent.outHitbox;
                        }

                        AddComponent(new Wire(this, new Line(WireStart, location), WireInputComponent, WireOutputComponent));
                        WirePlacementMode = false;
                        WireStart = null;
                        WireInputComponent = null;
                        WireOutputComponent = null;
                        WireOutputHitbox = null;
                        WireInputHitbox = null;
                    }
                    else
                    {
                        SelectComponent(location);
                        Draw(true);
                    }
                    break;
                case MouseKey.Right:
                    if (WirePlacementMode)
                    {
                        WirePlacementMode = false;
                        WireStart = null;
                        WireInputComponent = null;
                        WireOutputComponent = null;
                        WireOutputHitbox = null;
                        WireInputHitbox = null;

                        Draw();
                    }
                    else
                    { 
                        foreach (Component c in components)
                            if (c is IClickable)
                                ((IClickable)c).Click(location, this);

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
                        WireOutputHitbox = null;
                        WireInputHitbox = null;
                    }
                    break;
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
                case Key.L:
                    AddComponent(new Light(this, closest));
                    break;
                case Key.Q:
                        AddComponent(new ANDGate(this, closest));
                        break;
                case Key.Plus:
                Scale(0.25f);
                    break;
                case Key.Minus:
                    Scale(-0.25f);
                    break;
                case Key.Up:
                    if (SelectedComponent != null)
                        SelectedComponent.Translate(this, Direction.Up, 1);
                    break;
                case Key.Down:
                    if (SelectedComponent != null)
                        SelectedComponent.Translate(this, Direction.Down, 1);
                        break;
                case Key.Right:
                    if (SelectedComponent != null)
                        SelectedComponent.Translate(this, Direction.Right, 1);
                        break;
                case Key.Left:
                    if (SelectedComponent != null)
                        SelectedComponent.Translate(this, Direction.Left, 1);
                        break;
            }
        }

        public uint GetGridInterval()
        {
            return Grid.Interval;
        }

        public void Scale(float scale)
        {
            if (ScaleFactor + scale > 0)
            {
                ScaleFactor += scale;
                Grid.Scale(scale);

                if (LastScaleFactor < ScaleFactor)
                    foreach (Component component in components)
                    {
                        component.Scale(this, true);
                    }
                else
                    foreach (Component component in components)
                    {
                        component.Scale(this, false);
                    }

                LastScaleFactor = ScaleFactor;

                Draw();
            }
        }
    }
}
