using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Html5TankGame.AI
{
    public enum TankAction { TankClockwise, TankCounterclockwise, TurretClockwise, TurretCounterclockwise, Shoot, MoveForward, MoveBackward };
    public interface ITankController
    {
        List<TankAction> Move(TankEnviroment enviroment);
    }
}
