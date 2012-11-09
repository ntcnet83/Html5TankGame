using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Html5TankGame.Contracts;

namespace Html5TankGame.AI
{
    public class TankEnviroment
    {
        public List<Tank> VisibleTanks { get; set; }
        public bool IsAimingAtTank { get; set; }
        public int Cooldown { get; set; }
        public Arena Arena { get; set; }
        public PointF Position { get; set; }
    }
}