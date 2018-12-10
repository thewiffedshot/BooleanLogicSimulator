using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryLogic
{
    public class Vector
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector(float x, float y, bool normalize = false)
        {
            X = x;
            Y = y;

            if (normalize)
            {
                Normalize(this);
            }
        }

        public static Vector operator +(Vector p1, Vector p2)
        {
            return new Vector(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Vector operator -(Vector p1, Vector p2)
        {
            return new Vector(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Vector operator *(int scalar, Vector p)
        {
            return new Vector(p.X * scalar, p.Y * scalar);
        }

        public static Vector operator *(Vector p, int scalar)
        {
            return new Vector(p.X * scalar, p.Y * scalar);
        }

        public static Vector operator /(int scalar, Vector p)
        {
            return new Vector(p.X / scalar, p.Y / scalar);
        }

        public static Vector operator /(Vector p, int scalar)
        {
            return new Vector(p.X / scalar, p.Y / scalar);
        }

        public static Vector operator *(float scalar, Vector p)
        {
            return new Vector((int)(p.X * scalar), (int)(p.Y * scalar));
        }

        public static Vector operator *(Vector p, float scalar)
        {
            return new Vector((int)(p.X * scalar), (int)(p.Y * scalar));
        }

        public static Vector operator /(float scalar, Vector p)
        {
            return new Vector((int)(p.X / scalar), (int)(p.Y / scalar));
        }

        public static Vector operator /(Vector p, float scalar)
        {
            return new Vector((int)(p.X / scalar), (int)(p.Y / scalar));
        }

        private static void Normalize(Vector vector)
        {
            float l = Vector.Length(vector);
            vector.X /= l;
            vector.Y /= l;
        }

        private static float Length(Vector vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }
    }
}
