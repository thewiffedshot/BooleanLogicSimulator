using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;
using Point = BinaryLogic.Point;

namespace BinaryLogic
{
    public enum Key { Up, Down, Left, Right, Shift, Control, Space, Q, W, E, R, T, Y }
    public enum MouseKey { Left, Right, Middle }

    public class Scene
    {
        IRenderer renderer;
        Grid grid;
        public static Point Offset { get; set; }
        public Color Background { get; set; }
        public List<Component> components = new List<Component>(0);
        public Component SelectedComponent { get; private set; }

        public Scene(Grid grid, Color background, IRenderer renderer)
        {
            this.grid = grid;
            Background = background;
            this.renderer = renderer;
        }

        public void SetRenderer(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public void AddComponent(Component component)
        {
            components.Add(component);
            Draw();
        }

        public void SelectComponent(Point location)
        {
            throw new NotImplementedException();
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
                default:
                throw new NotImplementedException();
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
    }
}
