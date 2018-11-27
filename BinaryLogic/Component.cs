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
        // Need to define component fields.

        public abstract void ChangeColor(Color color);
        public abstract void Deselect();
        public abstract void Draw(IRenderer renderer);
        public abstract void Select(int mouseX, int mouseY);

        public void Delete(Scene scene)
        {
            scene.components.Remove(this);
        }
    }
}
