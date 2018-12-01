﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BinaryLogic
{
    public class ComponentHitbox
    {
        Rectangle hitbox;

        public ComponentHitbox(Rectangle rectangle)
        {
            hitbox = rectangle;
        }

        public void Translate(Direction direction, float units = 1)
        {
            hitbox.Move(direction, units);
        }
    }
}
