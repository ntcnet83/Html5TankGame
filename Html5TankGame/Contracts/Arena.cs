using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Html5TankGame.Contracts
{
    public class Arena
    {
        public Arena()
        {
            Border = new RectangleF((Game.ARENA_MARGIN / 2), (Game.ARENA_MARGIN / 2), Game.ARENA_WIDTH - (Game.ARENA_MARGIN / 2), Game.ARENA_HEIGHT - (Game.ARENA_MARGIN / 2));
            Boundary = new RectangleF(0, 0, Game.ARENA_WIDTH, Game.ARENA_HEIGHT);
        }

        public RectangleF Boundary { get; set; }
        public RectangleF Border { get; set; }

        public bool IsOutOfArena(float x, float y)
        {
            return !Boundary.Contains((float)x, (float)y);
        }
    }
}