using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;

namespace BinaryLogic
{
    public class Scene
    {
        IRenderer renderer;
        Grid grid;
        public static Point Offset { get; set; }
        public Color Background { get; set; }
        public List<Component> components;

        public Scene(Grid grid, Color background, IRenderer renderer)
        {
            this.grid = grid;
            Background = background;
            this.renderer = renderer;
        }

        void AddComponent(Component component)
        {
            components.Add(component);
            Draw();
        }

        void Draw()
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
            grid.Draw(renderer, Background);
        }
    }
}
