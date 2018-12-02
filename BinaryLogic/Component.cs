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
        public uint ID { get; set; }
        public Point Position { get; protected set; }
        public ComponentHitbox hitbox;
        protected InHitbox[] inHitboxes;
        protected OutHitbox outHitbox;

        public Color Color { get; protected set; }
        public readonly static Color DefaultColor = Color.Black;

        public bool Signal { get; protected set; }
        public List<Component>[] inputs;
        public List<Component> outputs;
         
        public float XIndent { get; protected set; }
        public float YIndent { get; protected set; }
        protected Line[] lines = new Line[0];
        protected Arc[] arcs = new Arc[0];
        protected Circle[] circles = new Circle[0];
        protected Rectangle[] rectangles = new Rectangle[0];
        protected uint Thickness { get; set; }


        public Component(ComponentType componentType, ComponentHitbox hitbox, uint thickness = 3, float xIndent = 3, float yIndent = 3)
        {
            Color = Color.Black;
            XIndent = xIndent;
            YIndent = yIndent;
            this.hitbox = hitbox;
            Thickness = thickness;

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
        }

        public virtual void ChangeColor(Color color)
        {
            Color = color;
        }
        
        public abstract void Deselect();
        public abstract void Draw(IRenderer renderer);
        public abstract bool Select(Point location);
        public abstract void Translate(Scene scene, Direction direction, uint units);
        public abstract void Scale(Scene scene);

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
        }
    }
}
