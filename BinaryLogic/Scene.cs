using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;

namespace BinaryLogic
{
    public enum Keys { Up, Down, Left, Right, Shift, Control, Space, Q, W, E, R, T, Y }

    public class Scene
    {
        IRenderer renderer;
        Grid grid;
        public static Point Offset { get; set; }
        public Color Background { get; set; }
        public List<Component> components = new List<Component>(0);

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

        public void KeyStroke(Keys key)
        {
            switch (key)
            {

            }
        }
        
        public float GetGridInterval()
        {
            return grid.Interval;
        }
    }
}
