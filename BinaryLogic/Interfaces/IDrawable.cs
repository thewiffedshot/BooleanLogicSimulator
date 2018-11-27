using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BinaryLogic.Interfaces
{
    public interface IDrawable
    {
        void Draw(IRenderer renderer);
        void ChangeColor(Color color);
    }
}
