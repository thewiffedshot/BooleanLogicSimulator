using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic;
using BinaryLogic.Interfaces;

namespace BooleanLogicSimulator
{
    class WinFormsRenderer : IRenderer
    {
        Scene currentScene;

        public WinFormsRenderer(Scene scene)
        {
            currentScene = scene;
        }

        public void ChangeScene(Scene scene)
        {
            currentScene = scene;
        }

        public void DrawArc(Arc arc)
        {
            throw new NotImplementedException();
        }

        public void DrawCircle(Circle circle)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(Arc line)
        {
            throw new NotImplementedException();
        }
    }
}
