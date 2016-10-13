using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DTileEngine
{
    class Tile
    {
        int x;
        int y;
        int graphic;
        int rotation;
        int tileType;

        public Tile(int x, int y) : this(x, y, 0)
        {}

        Tile(int x, int y, int graphic)
        {
            this.x = x;
            this.y = y;
            this.graphic = graphic;
        }

    }

   
}
