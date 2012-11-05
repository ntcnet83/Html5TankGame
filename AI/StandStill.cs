using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Html5TankGame.AI
{
    public class StandStill : ITankController
    {
        public List<TankAction> Move(TankEnviroment enviroment)
        {
            List<TankAction> ret = new List<TankAction>();

            if (enviroment.IsAimingAtTank)
            {
                ret.Add(TankAction.Shoot);
            }
            else
            {
                ret.Add(TankAction.TurretClockwise);
            }

            return ret;
        }
    }
}