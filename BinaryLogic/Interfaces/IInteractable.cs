﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryLogic.Interfaces
{
    public interface IInteractable : ISelectable, IDrawable
    {
        void Delete(Scene scene);
    }
}
