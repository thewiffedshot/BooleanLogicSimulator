using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;
using BinaryLogic.Components;
using Point = BinaryLogic.Point;

namespace BinaryLogic
{
    public enum Key { Up, Down, Left, Right, Shift, Control, Space, Q, W, E, R, T, Y, Invalid }
    public enum MouseKey { Left, Right, Middle }

    public class Scene
    {
        IRenderer renderer;
        Grid grid;
        public float ScaleFactor { get; private set; }
        public Point Offset { get; set; }
        public Color Background { get; set; }
        public List<Component> components = new List<Component>(0);
        public Component SelectedComponent { get; private set; }

        public Scene(Grid grid, Color background, IRenderer renderer)
        {
            this.grid = grid;
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
            uint id = 0;

            foreach (Component c in components)
                id++;

            component.ID = id;

            components.Add(component);
            Draw();
        }

        public void SelectComponent(Point location)
        {
            Component selected = components
                                 .Where(c => c.Select(location))
                                 .OrderBy(c => c.ID)
                                 .LastOrDefault();

            if (SelectedComponent == selected)
                DeselectComponent(selected);
            else
                SelectedComponent = selected;
        }

        public void DeselectComponent(Component component)
        {
            SelectedComponent = null;
            component.ChangeColor(Component.DefaultColor);
        }

        public void Draw()
        {
            Clear();
            
            foreach (Component component in components)
            {
                component.Draw(renderer);
            }
        }

        public void Clear()
        {
            renderer.Clear(Background);
            grid.Draw(renderer);
        }

        public void MouseClick(MouseKey key, Point location)
        {
            switch (key)
            {
                case MouseKey.Left:
                    SelectComponent(location);
                    break;
                default:
                throw new NotImplementedException();
            }
        }

        public void KeyStroke(Key key, Point mouseLocation)
        {
            float minDistance = float.MaxValue;
            Point closest = null;

            for (uint y = 0; y < grid.Field.points.GetLength(1); y++)
                for (uint x = 0; x < grid.Field.points.GetLength(0); x++)
                {
                    float distance = Point.Distance(grid.Field.points[x, y], mouseLocation);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closest = grid.Field.points[x, y];
                    }
                }
            
            switch (key)
            {
                case Key.T:
                    AddComponent(new Switch(this, closest));
                    break;
            }
        }

        public void KeyStroke(Key key)
        {
            switch (key)
            {
                default:
                    throw new NotImplementedException();
            }
        }

        public float GetGridInterval()
        {
            return grid.Interval;
        }

        public void Scale(float scale)
        {
            if (scale < 0)
                throw new ArgumentOutOfRangeException();

            ScaleFactor = scale;
            grid.Scale(scale);

            Draw();
        }
    }
}
