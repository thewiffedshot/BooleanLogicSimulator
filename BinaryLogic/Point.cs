using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryLogic
{
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point operator *(float scalar, Point p)
        {
            return new Point(p.X * scalar, p.Y * scalar);
        }

        public static Point operator *(Point p, float scalar)
        {
            return new Point(p.X * scalar, p.Y * scalar);
        }

        public static float Distance(Point p1, Point p2)
        {
            Point p = p1 - p2;

            return (float)Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }
    }
}
