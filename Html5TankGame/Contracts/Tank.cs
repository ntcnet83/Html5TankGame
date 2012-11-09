using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Html5TankGame.AI;

namespace Html5TankGame.Contracts
{
    public class Tank : GameElement
    {
        public float TurretAngle { get; set; }

        private const int PROPORTIONED_TANK_SIZE = Game.TANK_SIZE * Game.TANK_BODY_SCALE;
        private ITankController _controller;
        public readonly string ControllerName;
        private int _cooldown = Game.MISSLE_COOLDOWN;

        public Tank(ITankController controller)
        {
            _controller = controller;
            ControllerName = controller.ToString();
        }

        private PointF NewShotLocation()
        {
            return new PointF(
                (int)(this.X + Math.Sin(this.TurretAngle) * this.Size * Game.TANK_TURRET_SCALE), 
                (int)(this.Y + Math.Cos(this.TurretAngle) * this.Size * Game.TANK_TURRET_SCALE));
        }

        internal Missile Move()
        {
            float ProposedX;
            float ProposedY;
            Missile ret = null;
            TankEnviroment env = SetupEnviroment();
            var movements = _controller.Move(env)
                .GroupBy(m => m)
                .Select(m => m.First());
            foreach (var m in movements)
            {
                switch (m)
                {
                    case TankAction.Shoot:
                        if (_cooldown == 0)
                        {
                            var newShot = NewShotLocation();
                            ret = new Missile()
                            {
                                Angle = this.TurretAngle,
                                X = newShot.X,
                                Y = newShot.Y,
                                R = this.R,
                                G = this.G,
                                B = this.B,
                            };
                            _cooldown = Game.MISSLE_COOLDOWN;
                        }
                        break;
                    case TankAction.TankClockwise:
                        this.Angle += Game.TANK_TURN_SPEED;
                        break;
                    case TankAction.TankCounterclockwise:
                        this.Angle -= Game.TANK_TURN_SPEED;
                        break;
                    case TankAction.TurretClockwise:
                        this.TurretAngle += Game.TURRET_TURN_SPEED;
                        break;
                    case TankAction.TurretCounterclockwise:
                        this.TurretAngle -= Game.TURRET_TURN_SPEED;
                        break;
                    case TankAction.MoveBackward:
                        ProposedX = this.X - (float)Math.Sin(this.Angle) * Game.TANK_SPEED;
                        ProposedY = this.Y - (float)Math.Cos(this.Angle) * Game.TANK_SPEED;
                        if (IsTankInbounds((float)ProposedX,(float) ProposedY))
                        {
                            this.X = ProposedX;
                            this.Y = ProposedY;
                        }
                        break;
                    case TankAction.MoveForward:
                        ProposedX = this.X + (float)Math.Sin(this.Angle) * Game.TANK_SPEED;
                        ProposedY = this.Y + (float)Math.Cos(this.Angle) * Game.TANK_SPEED;
                        if (IsTankInbounds((float)ProposedX,(float) ProposedY))
                        {
                            this.X = ProposedX;
                            this.Y = ProposedY;
                        }
                        break;
                }
            }

            if (_cooldown > 0)
                _cooldown--;

            while (Angle > 2 * Math.PI)
            {
                Angle -= 2 * (float)Math.PI;
            }
            while (Angle < 0)
            {
                Angle += 2 * (float)Math.PI;
            }
            while (TurretAngle > 2 * Math.PI)
            {
                TurretAngle -= 2 * (float)Math.PI;
            }
            while (TurretAngle < 0)
            {
                TurretAngle += 2 * (float)Math.PI;
            }

            return ret;
        }

        private TankEnviroment SetupEnviroment()
        {
            var ret = new TankEnviroment()
            {
                IsAimingAtTank = false,
                Arena = Game.Arena,
                Cooldown = _cooldown,
                Position = new PointF((float)X, (float)Y),
                VisibleTanks = new List<Tank>()
            };

            var newShot = NewShotLocation();
            while (Game.Arena.Border.Contains(newShot))
            {
                if (TrackPathForTarget(ref newShot))
                {
                    ret.IsAimingAtTank = true;
                    break;
                }
                newShot.X += (float)Math.Sin(TurretAngle) * 2;
                newShot.Y += (float)Math.Cos(TurretAngle) * 2;
            }

            return ret;
        }

        private static bool TrackPathForTarget(ref PointF newShot)
        {
            foreach (var tank in Game.ActiveTanks)
            {
                if (DetectCollision((int)tank.X, (int)tank.Y, (int)newShot.X, (int)newShot.Y))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool DetectCollision(int TankX, int TankY, int MissleX, int MissleY)
        {
            return new Rectangle(
                TankX - Game.TANK_SIZE, TankY - Game.TANK_SIZE, 
                Tank.PROPORTIONED_TANK_SIZE, Tank.PROPORTIONED_TANK_SIZE).Contains(MissleX, MissleY);
            
        }

        private static bool IsTankInbounds(float ProposedX, float ProposedY)
        {
            return Game.Arena.Border.Contains(ProposedX - PROPORTIONED_TANK_SIZE, ProposedY - PROPORTIONED_TANK_SIZE) &&
                                        Game.Arena.Border.Contains(ProposedX + PROPORTIONED_TANK_SIZE, ProposedY - PROPORTIONED_TANK_SIZE) &&
                                        Game.Arena.Border.Contains(ProposedX - PROPORTIONED_TANK_SIZE, ProposedY + PROPORTIONED_TANK_SIZE) &&
                                        Game.Arena.Border.Contains(ProposedX + PROPORTIONED_TANK_SIZE, ProposedY + PROPORTIONED_TANK_SIZE);
        }
        

        public List<Missile> HitTest(List<Tank> enemyTanks)
        {
            var hits = new List<Missile>();
            //for (var missileIndex = _missiles.Count - 1; missileIndex >= 0; missileIndex--)
            //{
            //    bool hit = false;
            //    var missile = _missiles[missileIndex];
            //    for (var enemyIndex = 0; enemyIndex < enemies.Count; enemyIndex++)
            //    {
            //        var enemy = enemies[enemyIndex];
            //        if (missile.X.IsBetween(enemy.X - 13, enemy.X + 13) && missile.Y.IsBetween(enemy.Y - 13, enemy.Y + 13))
            //        {
            //            hit = true;
            //            hits.Add(new Missile() { Angle = 10, X = enemy.X, Y = enemy.Y });
            //            enemies.Remove(enemy);
            //        }
            //    }
            //    if (hit)
            //        _missiles.Remove(missile);
            //}
            return hits;
        }
    }
}