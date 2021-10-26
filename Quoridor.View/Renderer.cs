using System;
using System.Collections.Generic;
using System.Text;

namespace Quoridor.View
{
    public class Renderer
    {
        protected int offX = 10;
        protected int offY = 2;

        public Renderer() { }

        public Renderer(int offX, int offY)
        {
            this.offX = offX;
            this.offY = offY;
        }
    }
}
