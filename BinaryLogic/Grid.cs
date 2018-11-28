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

            Interval -= (uint)windowSize.X % Interval;

        }

        public Grid(Point windowSize, Color color)
        {
            Interval = 5;
            Color = color;
            Thickness = GridThickness.Normal;

            Interval -= (uint)windowSize.X % Interval;

        }

        public Grid(Point windowSize)
        {
            Interval = 5;
            Color = Color.Black;
            Thickness = GridThickness.Normal;

            Interval -= (uint)windowSize.X % Interval;
        }

        public void ChangeColor(Color color)
        {
            Color = color;
        }

        public void Draw(IRenderer renderer, Color background)
        {
            Clear(renderer, background);

            for (uint y = 0; y < WindowSize.Y; y += Interval)
            {
                renderer.DrawLine(new Line(new Point(0, (int)y), new Point(WindowSize.X, (int)y)), Color, (uint)Thickness);
            }

            for (uint x = 0; x < WindowSize.X; x += Interval)
            {
                renderer.DrawLine(new Line(new Point((int)x, 0), new Point((int)x, WindowSize.Y)), Color, (uint)Thickness);
            }
        }

        public void Clear(IRenderer renderer, Color background)
        {
            renderer.Clear(background);
        }
    }
}
