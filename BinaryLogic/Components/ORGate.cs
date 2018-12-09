using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;

namespace BinaryLogic.Components
{
    public class ORGate : Component
    {
        public ORGate(ComponentType componentType, ComponentHitbox hitbox) : base(componentType, hitbox, 3)
        {
        }

        public override void ChangeColor(Color color)
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

        public override void Process()
        {
            throw new NotImplementedException();
        }

        public override void Scale(Scene scene)
        {
            throw new NotImplementedException();
        }

        public override bool Select(Point location, Scene sender)
        {
            throw new NotImplementedException();
        }

        public override void Translate(Scene scene, Direction direction, uint units = 1)
        {
            throw new NotImplementedException();
        }

        public override List<Component> Transmit(List<Component> outputs, bool signal)
        {
            throw new NotImplementedException();
        }
    }
}
