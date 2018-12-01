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
    public enum ComponentType { AND, OR, XOR, NOT, Wire, Switch, Button, Light };

    public abstract class Component : IInteractable
    {
        public uint ID { get; private set; }
        public ComponentHitbox hitbox;

        public bool Signal { get; protected set; }
        public List<Component>[] inputs;
        public List<Component> outputs;
         
        Line[] lines = new Line[0];
        Arc[] arcs = new Arc[0];
        Circle[] circles = new Circle[0];


        public Component(ComponentType componentType, ComponentHitbox hitbox)
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
                case ComponentType.Switch:
                    inputs = new List<Component>[0];
                    break;
                case ComponentType.Button:
                    inputs = new List<Component>[0];
                    break;
                case ComponentType.Light:
                    inputs = new List<Component>[1];
                    inputs[0] = new List<Component>(0);
                    break;
                default:
                    throw new ArgumentException("No such component type registered in enumeration.");
            }

            this.hitbox = hitbox;
        }

        public abstract void ChangeColor(Color color);
        public abstract void Deselect();
        public abstract void Draw(IRenderer renderer);
        public abstract void Select(Point location);

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

        public void Translate(Scene scene, Direction direction, float units)
        {
            hitbox.Translate(direction, scene.GetGridInterval() * units);
        }
    }
}
