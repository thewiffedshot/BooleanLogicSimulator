using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic;
using BinaryLogic.Interfaces;
using System.Drawing;

namespace BinaryLogic
{
    public abstract class Component : IInteractable
    {
        Rectangle rectangle = new Rectangle(new Point(0,0), 10, 10);

        public abstract void ChangeColor(Color color);
        public abstract void Deselect();
        public abstract void Draw(IGraphics graphics);
        public abstract void Select(int mouseX, int mouseY);
    }
}
