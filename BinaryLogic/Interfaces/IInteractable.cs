using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryLogic.Interfaces
{
    public interface IInteractable : ISelectable, IDrawable
    {
        void Delete(List<Component> components);
        void Translate(Direction direction, float units);
    }
}
