using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;

namespace BinaryLogic.Components
{
    public class Button : Component, IHoldable
    {
        public override void ChangeColor(Color color)
        {
            throw new NotImplementedException();
        }

        public override void Deselect()
        {
            throw new NotImplementedException();
        }

        public override void Draw(IGraphics graphics)
        {
            throw new NotImplementedException();
        }

        public void Pressed(int mouseX, int mouseY)
        {
            throw new NotImplementedException();
        }

        public void Released()
        {
            throw new NotImplementedException();
        }

        public override void Select(int mouseX, int mouseY)
        {
            throw new NotImplementedException();
        }
    }
}
