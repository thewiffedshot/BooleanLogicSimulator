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

    internal abstract class Component : IInteractable, IDisposable
    {
        public uint ID { get; set; }
        public uint Level { get; set; } = 0;
        public bool Checked { get; set; } = false;
        public Point Position { get; protected set; }
        public Point StartPosition { get; set; }
        public ComponentHitbox hitbox;
        public InHitbox[] inHitboxes = new InHitbox[0];
        public OutHitbox outHitbox;
        public delegate bool clickAction(Point location);

        public Color Color { get; protected set; }
        public readonly static Color DefaultColor = Color.Black;

        public bool Signal { get; set; }
        public List<Component>[] inputs;
        public List<Component> outputs = new List<Component>(0);

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
        public abstract bool Select(Point location, Scene sender);
        public abstract void Translate(Scene scene, Direction direction, uint units);
        public abstract void Scale(Scene scene, bool zoom);

        public InHitbox InputClicked(Point location)
        {
            foreach (InHitbox hitbox in inHitboxes)
            {
                if (hitbox != null)
                    if (hitbox.Clicked(location))
                    {
                        return hitbox;
                    }
            }

            return null;
        }

        public OutHitbox OutputClicked(Point location)
        {
            if (outHitbox == null) return null;

            if (outHitbox.Clicked(location))
            {
                return outHitbox;
            }
            
            return null;
        }

        public abstract void Process(Scene scene);

        public void SetLevel(uint level)
        {
            if (Checked) return;

            Level = level;

            Checked = true;

            foreach (Component c in outputs)
            {
                if (!(c is Wire) || !(this is Wire))
                    c.SetLevel(level + 1);
                else
                    c.SetLevel(level);
            }
        }

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

        public void Dispose()                               // Might need to add edge cases for some components.
        {
            if (this is Wire)
            {
                foreach (Component component in outputs)
                    if (component is Wire)
                    {
                        component.inputs[0].Remove(this);
                    }

                foreach (Component component in inputs[0])
                    if (component is Wire)
                    {
                        component.outputs.Remove(this);
                    }

                foreach (Component source in ((Wire)this).sources)
                    foreach (Component component in source.outputs)
                        if (component is Wire)
                            ((Wire)component).Propagate(new List<Wire>(0), source, false, true);
            }
            else
            {
                foreach (Component component in outputs)
                    if (component is Wire)
                        ((Wire)component).Propagate(new List<Wire>(), this, true);
            }
        }
    }
}
