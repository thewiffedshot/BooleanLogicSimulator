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
        public uint StartInterval { get; private set; }
        public uint Interval { get; private set; }
        public GridThickness Thickness { get; set; }
        public PointField Field { get; private set; }
        private float ScaleFactor { get; set; }

        public Grid(Point windowSize, uint interval, Color color, GridThickness thickness)
        {
            ScaleFactor = 1;
            StartInterval = interval;
            Interval = interval;
            Color = color;
            Thickness = thickness;
            WindowSize = windowSize;

            Field = new PointField(this);
        }

        public Grid(Point windowSize, Color color)
        {
            ScaleFactor = 1;
            StartInterval = 35;
            Interval = StartInterval;
            Color = color;
            Thickness = GridThickness.Small;
            WindowSize = windowSize;

            Field = new PointField(this);
        }

        public Grid(Point windowSize)
        {
            ScaleFactor = 1;
            StartInterval = 35;
            Interval = StartInterval;
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
            ScaleFactor += scale;

            Interval = (uint)(ScaleFactor * StartInterval);
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

        public Point SnapToGrid(Point point)
        {
            float minDistance = float.MaxValue;
            Point closest = null;

            for (uint y = 0; y < Field.points.GetLength(1); y++)
                for (uint x = 0; x < Field.points.GetLength(0); x++)
                {
                    float distance = Point.Distance(Field.points[x, y], point);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closest = Field.points[x, y];
                    }
                }

            return closest;
        }

        public void Clear(IRenderer renderer, Color background)
        {
            renderer.Clear(background);
        }
    }
}
