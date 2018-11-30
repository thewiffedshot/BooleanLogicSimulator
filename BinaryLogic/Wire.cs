using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;
using BinaryLogic;

namespace BinaryLogic
{
    class Wire : Component
    {
        public Wire() : base(ComponentType.Wire)
        {
        }

        public override List<Component> Transmit(List<Component> outputs, bool signal)
        {
            List<Component> found = new List<Component>(0);

            if (outputs == null) return found;
            
            foreach (Component output in outputs)
            {
                if (output is Wire)
                {
                    found.Concat(((Wire)output).Transmit(found, signal));
                }
                else
                {
                    var result = new List<Component>();
                    result.Add(output);

                    if (output.outputs != null)
                        output.Set(signal, this);

                    return result;
                }
            }

            return null;
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

        public override void Select(int mouseX, int mouseY)
        {
            throw new NotImplementedException();
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
}
