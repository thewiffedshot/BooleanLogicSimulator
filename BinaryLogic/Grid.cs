using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BinaryLogic
{
    public enum GridThickness { Small, Normal, Thicker, Thickest }
    public enum Direction { Right, Left, Up, Down }

    public class Grid
    {
        public static uint scale { get; set; }
        public static Point offset { get; set; }
    }
}
