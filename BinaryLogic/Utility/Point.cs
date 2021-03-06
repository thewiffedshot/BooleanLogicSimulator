﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryLogic
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point()
        {
            X = 0;
            Y = 0;
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point operator +(Point p1, Vector p2)
        {
            return new Point((int)(p1.X + p2.X), (int)(p1.Y + p2.Y));
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point operator -(Point p1, Vector p2)
        {
            return new Point((int)(p1.X - p2.X), (int)(p1.Y - p2.Y));
        }

        public static Point operator *(int scalar, Point p)
        {
            return new Point(p.X * scalar, p.Y * scalar);
        }

        public static Point operator *(Point p, int scalar)
        {
            return new Point(p.X * scalar, p.Y * scalar);
        }

        public static Point operator /(int scalar, Point p)
        {
            return new Point(p.X / scalar, p.Y / scalar);
        }

        public static Point operator /(Point p, int scalar)
        {
            return new Point(p.X / scalar, p.Y / scalar);
        }

        public static Point operator *(float scalar, Point p)
        {
            return new Point((int)(p.X * scalar), (int)(p.Y * scalar));
        }

        public static Point operator *(Point p, float scalar)
        {
            return new Point((int)(p.X * scalar), (int)(p.Y * scalar));
        }

        public static Point operator /(float scalar, Point p)
        {
            return new Point((int)(p.X / scalar), (int)(p.Y / scalar));
        }

        public static Point operator /(Point p, float scalar)
        {
            return new Point((int)(p.X / scalar), (int)(p.Y / scalar));
        }

        public static float Distance(Point p1, Point p2)
        {
            Point p = p1 - p2;

            return (float)Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }

        public static int operator *(Point v1, Point v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }
    }
}
