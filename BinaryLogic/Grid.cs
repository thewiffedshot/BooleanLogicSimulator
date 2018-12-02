using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using BinaryLogic.Interfaces;
using Point = BinaryLogic.Point;

namespace BinaryLogic
{
    public enum GridThickness { Small = 1, Normal, Thicker, Thickest }
    public enum Direction { Right, Left, Up, Down }

    public class Grid
    {
        public Point WindowSize { get; private set; }
        public static Color Color { get; set; }
        public uint Interval { get; private set; }
        public GridThickness Thickness { get; set; }
        public PointField Field { get; private set; } // Probably will be it's own type

        public Grid(Point windowSize, uint interval, Color color, GridThickness thickness)
        {
            Interval = interval;
            Color = color;
            Thickness = thickness;
            WindowSize = windowSize;

            Field = new PointField(this);
        }

        public Grid(Point windowSize, Color color)
        {
            Interval = 35;
            Color = color;
            Thickness = GridThickness.Small;
            WindowSize = windowSize;

            Field = new PointField(this);
        }

        public Grid(Point windowSize)
        {
            Interval = 35;
            Color = Color.Black;
            Thickness = GridThickness.Small;
            WindowSize = windowSize;

            Field = new PointField(this);
        }

        public void ChangeColor(Color color)
        {
            Color = color;
        }

        public void Scale(float scale)
        {
            if (scale < 0)
                throw new ArgumentException();

            Interval = (uint)(Interval * scale);
            Field = new PointField(this);
        }

        public void Draw(IRenderer renderer)
        {
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
