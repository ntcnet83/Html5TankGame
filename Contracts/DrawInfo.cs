using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Html5TankGame.Contracts
{
    public class DrawInfo
    {
        public Tank[] Tanks;
        public Missile[] Missles;
        public Arena Arena;
        public string Debug;
        public ScoreInfo[] Scores;
    }
}