using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;

namespace BinaryLogic.Components
{
    public class Switch : Component, IClickable
    {
        public override void ChangeColor(Color color)
        {
            throw new NotImplementedException();
        }

        public void Click(int mouseX, int mouseY)
        {
            throw new NotImplementedException();
        }

        public override void Deselect()
        {
            throw new NotImplementedException();
        }

        public override void Draw(IRenderer renderer)
        {
            throw new NotImplementedException();
        }

        public override void Select(int mouseX, int mouseY)
        {
            throw new NotImplementedException();
        }
    }
}
