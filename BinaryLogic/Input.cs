using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryLogic
{
    public class Input
    {
        public List<Component> components = new List<Component>(0);
        public bool Signal { get; set; }

        void Set()
        {
            foreach (Component component in components)
            {
                Signal = false;

                foreach (Output output in component.outputs)
                {
                    if (output.Signal == true)
                    {
                        Signal = true;
                    }
                }
            }
        }
    }
}
