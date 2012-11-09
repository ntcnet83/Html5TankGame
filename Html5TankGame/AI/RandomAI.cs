using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Html5TankGame.AI
{
    public class RandomAI : ITankController
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
            
            ret.Add(TankAction.MoveForward);
            ret.Add(TankAction.TankCounterclockwise);

            return ret;
        }
    }
}