using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;
using Point = BinaryLogic.Point;

namespace BinaryLogic.Components
{
    public class Switch : Component, IClickable
    {
        public Switch(Scene scene, Point position)
            : base(ComponentType.Switch, new ComponentHitbox(new Rectangle(position, 2 * scene.GetGridInterval(), 2 * scene.GetGridInterval())), 3)
        {

        }

        public override void ChangeColor(Color color)
        {
            throw new NotImplementedException();
        }

        public void Click(Point location)
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

        public override bool Select(Point location)
        {
            throw new NotImplementedException();
        }

        public override void Translate(Direction direction, float units)
        {
            throw new NotImplementedException();
        }

        public override List<Component> Transmit(List<Component> outputs, bool signal)
        {
            throw new NotImplementedException();
        }
    }
}
