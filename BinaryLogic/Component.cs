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
        public uint ID { get; private set; }
        public bool Signal { get; private set; }
        public List<Component> inputs;
        public List<Component> outputs;

        public abstract void ChangeColor(Color color);
        public abstract void Deselect();
        public abstract void Draw(IRenderer renderer);
        public abstract void Select(int mouseX, int mouseY);

        public void Set(bool signal, Component sender)
        {
            Signal = signal;
            Transmit(outputs, Signal);
        }

        public abstract List<Component> Transmit(List<Component> outputs, bool signal);

        public void Delete(Scene scene)
        {
            scene.components.Remove(this);
        }
    }
}
