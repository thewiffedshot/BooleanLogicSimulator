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
    public enum ComponentType { AND, OR, XOR, NOT, Wire };

    public abstract class Component : IInteractable
    {
        public uint ID { get; private set; }
        public bool Signal { get; protected set; }
        public List<Component>[] inputs;
        public List<Component> outputs;

        public Component(ComponentType componentType)
        {
            switch (componentType)
            {
                case ComponentType.AND:
                    inputs = new List<Component>[2];
                    inputs[0] = new List<Component>(0);
                    inputs[1] = new List<Component>(0);
                    break;
                case ComponentType.OR:
                    inputs = new List<Component>[2];
                    inputs[0] = new List<Component>(0);
                    inputs[1] = new List<Component>(0);
                    break;
                case ComponentType.XOR:
                    inputs = new List<Component>[2];
                    inputs[0] = new List<Component>(0);
                    inputs[1] = new List<Component>(0);
                    break;
                case ComponentType.NOT:
                    inputs = new List<Component>[1];
                    inputs[0] = new List<Component>(0);
                    break;
                case ComponentType.Wire:
                    inputs = new List<Component>[1];
                    inputs[0] = new List<Component>(0);
                    break;
                default:
                    throw new ArgumentException("No such component type registered in enumeration.");
            }
        }

        public abstract void ChangeColor(Color color);
        public abstract void Deselect();
        public abstract void Draw(IRenderer renderer);
        public abstract void Select(int mouseX, int mouseY);

        public abstract void Process();

        public void Set(bool signal)
        {
            Signal = signal;
            Process();
            Transmit(outputs, Signal);
        }

        public abstract List<Component> Transmit(List<Component> outputs, bool signal);

        public void Delete(Scene scene)
        {
            scene.components.Remove(this);

            foreach (List<Component> inputSlot in inputs)
                foreach (Component component in inputSlot)
                {
                    component.outputs.Remove(this);
                }
            
            foreach (Component component in outputs)
                foreach (List<Component> inputSlot in component.inputs)
                {
                    inputSlot.Remove(this);
                }

            scene.Draw();
        }
    }
}
