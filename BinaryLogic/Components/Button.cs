﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryLogic.Interfaces;

namespace BinaryLogic.Components
{
    internal class Button : Component, IHoldable
    {
        public Button(ComponentType componentType, ComponentHitbox hitbox)  // TODO: Implement button.
            : base(componentType, hitbox) 
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

        public void Pressed(Point location)
        {
            throw new NotImplementedException();
        }

        public override void Process(Scene scene)
        {
            throw new NotImplementedException();
        }

        public void Released()
        {
            throw new NotImplementedException();
        }

        public override void Scale(Scene scene, bool zoom)
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
    }
}
