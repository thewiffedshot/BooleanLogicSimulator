using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using BinaryLogic.Interfaces;

namespace BinaryLogic
{
    public enum GridThickness { Small = 1, Normal, Thicker, Thickest }
    public enum Direction { Right, Left, Up, Down }

    public class Grid
    {
        public Point WindowSize { get; private set; }
        public static uint Scale { get; set; }
        public static Color Color { get; set; }
        public uint Interval { get; private set; }
        public GridThickness Thickness { get; set; }
        public Point[][] PointField { get; private set; } // Probably will be it's own type

        public Grid(Point windowSize, uint interval, Color color, GridThickness thickness)
        {
            Interval = interval;
            Color = color;
            Thickness = thickness;

            // TODO.

        }

        public Grid(Point windowSize, Color color)
        {
            Interval = 15;
            Color = color;
            Thickness = GridThickness.Normal;

            // TODO.

        }

        public Grid(Point windowSize)
        {
            Interval = 15;
            Color = Color.Gray;
            Thickness = GridThickness.Normal;

            // TODO.
        }

        public void ChangeColor(Color color)
        {
            throw new NotImplementedException();
        }

        public void Draw(IRenderer renderer, Color background)
        {
            Clear(renderer, background);

            // TODO.
            throw new NotImplementedException();
        }

        public void Clear(IRenderer renderer, Color background)
        {
            renderer.Clear(background);
        }
    }
}
