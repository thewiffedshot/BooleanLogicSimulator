﻿using System;
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
    public enum Key { Up, Down, Left, Right, Shift, Control, Space, Q, W, E, R, T, Y, L, Plus, Minus, Delete, Invalid }
    public enum MouseKey { Left, Right, Middle, Invalid }

    public class Scene 
    {
        public IRenderer Renderer { get; private set; }
        public Grid Grid { get; set; }
        public float ScaleFactor { get; private set; }
        private float LastScaleFactor { get; set; }
        public Point Offset { get; set; }
        public Color Background { get; set; }
        internal List<Component> components = new List<Component>(0);
        internal Component SelectedComponent { get; private set; }

        public bool WirePlacementMode { get; private set; }
        public bool WireInput { get; private set; }
        public Point WireStart { get; private set; }
        public Point MouseLocation { get; private set; }
        internal Component WireInputComponent { get; set; }
        internal Component WireOutputComponent { get; set; }
        internal InHitbox WireInputHitbox { get; set; }
        internal OutHitbox WireOutputHitbox { get; set; }

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
            foreach (Component component in components)  // Recursively set levels of components
            {                                            // starting from level 0 (signal sources)
                if (component is Switch)
                {
                    component.SetLevel(0);
                }

                else if (component is Button)
                {
                    component.SetLevel(0);
                }

                else if (component is Clock)
                {
                    component.SetLevel(0);
                }
            }

            uint maxLevel = components
                            .OrderBy(c => c.Level)
                            .Last().Level;

            var ordered   = components
                            .OrderBy(c => c.Level)
                            .ToList();

            foreach (Component component in ordered)
                if (!(component is Wire))
                    component.Process(this);

            foreach (Component component in components)
                component.Checked = false;
        }

        public void SetRenderer(IRenderer renderer)
        {
            Renderer = renderer;
        }

        internal void AddComponent(Component component)
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

        internal void DeselectComponent(Component component)
        {
            if (component != null)
            {
                if (component is Wire)
                {
                    component.ChangeColor(component.Signal ? Color.Red : Color.Black);
                }
                else
                {
                    component.ChangeColor(Component.DefaultColor);
                    component.ChangeColor(Component.DefaultColor);
                }

                SelectedComponent = null;
            }
        }
        
        internal void RemoveComponent(Component component)
        {
            if (component != null)
            {
                //component.Dispose();
                foreach (var cs in component.inputs)
                    foreach (var c in cs)
                        if (c is Wire)
                        {
                            ((Wire)c).InConnected = null;
                        }

                foreach (var c in component.outputs)
                    if (c is Wire)
                    {
                        ((Wire)c).OutConnected = null;
                    }

                components.Remove(component);

                foreach (Component c in components)
                    c.Process(this);

                Update();
                Draw();
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

        internal void WireMode(Point start, Component sender, object hitbox, bool input = false)
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
                        foreach (Component component in components)
                            component.Select(location, this);

                        if (WireOutputComponent == null)
                        {
                            WireOutputComponent = components
                                                    .Where(c => c.InputClicked(location) != null)
                                                    .LastOrDefault();           
                        }     
                        
                        if (WireInputComponent == null)
                        {
                            WireInputComponent = components
                                                    .Where(c => c.OutputClicked(location) != null)
                                                    .LastOrDefault();
                        }

                        if (WireInputHitbox == null && WireOutputComponent is Wire)
                        {
                            WireInputHitbox = WireOutputComponent.inHitboxes[0];
                        }
                        else if (WireInputHitbox == null && WireOutputComponent != null)
                        {
                            foreach (InHitbox hitbox in WireOutputComponent.inHitboxes)
                            {
                                if (hitbox.Clicked(location))
                                {
                                    WireInputHitbox = hitbox;
                                }
                            }
                        }

                        if (WireOutputHitbox == null)
                            WireOutputHitbox = WireInputComponent.outHitbox;

                        Wire wire = new Wire(this, new Line(WireStart, location), WireInputComponent, WireOutputComponent);

                        if (WireInputComponent is Wire && WireOutputComponent is Wire)
                        {
                            //((Wire)WireInputComponent).InConnected = wire.inHitboxes[0];
                            wire.outHitbox = ((Wire)WireInputComponent).OutConnected;
                            wire.inHitboxes[0] = ((Wire)WireOutputComponent).InConnected;

                            wire.InConnected = wire.inHitboxes[0];
                            //wire.OutConnected = wire.outHitbox;
                        }

                        AddComponent(wire);

                        // Update output component on wire creation.
                        if (WireOutputComponent != null && !(WireOutputComponent is Wire))
                            WireOutputComponent.Process(this);

                        EndPlacementMode();
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
                        EndPlacementMode();

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
                        EndPlacementMode();
                    }
                    break;
            }
        }

        void EndPlacementMode()
        {
            WirePlacementMode = false;
            WireStart = null;
            WireInputComponent = null;
            WireOutputComponent = null;
            WireOutputHitbox = null;
            WireInputHitbox = null;
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
                case Key.W:
                    AddComponent(new NOTGate(this, closest));
                    break;
                case Key.E:
                    AddComponent(new ORGate(this, closest));
                    break;
                case Key.R:
                    AddComponent(new XORGate(this, closest));
                    break;
                case Key.Y:
                    AddComponent(new Clock(this, closest));
                    break;
                case Key.Plus:
                    Scale(0.25f);
                    break;
                case Key.Minus:
                    Scale(-0.25f);
                    break;
                case Key.Up:
                    if (SelectedComponent != null && !(SelectedComponent is Wire))
                        SelectedComponent.Translate(this, Direction.Up, 1);
                    else
                        TranslateScene(Direction.Down);
                    break;
                case Key.Down:
                    if (SelectedComponent != null && !(SelectedComponent is Wire))
                        SelectedComponent.Translate(this, Direction.Down, 1);
                    else
                        TranslateScene(Direction.Up);
                    break;
                case Key.Right:
                    if (SelectedComponent != null && !(SelectedComponent is Wire))
                        SelectedComponent.Translate(this, Direction.Right, 1);
                    else
                        TranslateScene(Direction.Left);
                    break;
                case Key.Left:
                    if (SelectedComponent != null && !(SelectedComponent is Wire))
                        SelectedComponent.Translate(this, Direction.Left, 1);
                    else
                        TranslateScene(Direction.Right);
                    break;
                case Key.Delete:
                    if (SelectedComponent != null)
                        RemoveComponent(SelectedComponent);
                    break;
            }
        }

        void TranslateScene(Direction direction, uint units = 1)
        {
            foreach (Component component in components)
                if (!(component is Wire))
                    component.Translate(this, direction, units);
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
                {
                    foreach (Component component in components)
                        if (!(component is Wire))
                            component.Scale(this, true);
                }
                else
                    foreach (Component component in components)
                        if (!(component is Wire))
                            component.Scale(this, false);

                LastScaleFactor = ScaleFactor;

                Draw();
            }
        }
    }
}
