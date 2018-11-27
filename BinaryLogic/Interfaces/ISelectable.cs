﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryLogic.Interfaces
{
    public interface ISelectable
    {
        void Select(int mouseX, int mouseY);
        void Deselect();
    }
}
