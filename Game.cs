using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Html5TankGame.AI;
using Html5TankGame.Contracts;

namespace Html5TankGame
{
    public class Game
    {
        #region Game Variable Constants
        //---Game---
        public static int SLEEP_DURATION = 30;
        //public static int NUM_TANKS = 2;
        private static bool WinCondition() { return _tanks.Count() == 1; }

        //---Tank---
        public static int TANK_SPEED = 2;
        public const int TANK_SIZE = 10;
        public static float TURRET_TURN_SPEED = .03f;
        public const float TANK_TURN_SPEED = .05f;
        public const int TANK_BODY_SCALE = 2;
        public const int TANK_TURRET_SCALE = 3;
        public const float TANK_HULL_SCALE = .5f;

        //---Missle---
        public const int MISSLE_COOLDOWN = 20;
        public const int MISSLE_SPEED = 5;
        public const int BULLET_SIZE = 3;


        //---Arena---
        public const int ARENA_HEIGHT = 500;
        public const int ARENA_WIDTH = 1000;
        public const int ARENA_MARGIN = 10;
        #endregion

        //Properties
        private static System.Random _random = new System.Random();
        private bool _stop = false;
        private bool _newGame;
        private static List<Tank> _tanks = new List<Tank>();
        private List<Missile> _missles = new List<Missile>();
        private List<ClientHandler> _handlers;
        private readonly static Arena _arena = new Arena();
        private static Game _game;
        public static readonly List<ScoreInfo> Scores = new List<ScoreInfo>();

        private Game()
        {
            _stop = false;
            _handlers = new List<ClientHandler>();
        }

        public static void StartGame()
        {
            _game = new Game();
            new Task(_game.Main).Start();
        }

        public static void EndGame()
        {
            _game.Stop();
        }

        public static void AddGameHandler(ClientHandler handler)
        {
            _game.AddHandler(handler);
        }

        public static int NumberOfShips
        {
            get { return _tanks.Count; }
        }

        public static Arena Arena
        {
            get { return _arena; }
        }

        public static List<Tank> ActiveTanks
        {
            get
            {
                return _tanks;
            }
        }

        public void Stop()
        {
            _stop = true;
        }

        public void AddHandler(ClientHandler handler)
        {
            _handlers.Add(handler);
        }

        public void Main()
        {
            while (!_stop)
            {
                SetupNewGame();
                while (!_newGame && !_stop)
                {
                    foreach (var tank in _tanks)
                    {
                        MoveTank(tank);
                    }
                    foreach (var missle in _missles)
                    {
                        MoveMissle(missle);
                        foreach (var tank in _tanks)
                        {
                            if (Tank.DetectCollision((int)tank.X, (int)tank.Y, (int)missle.X, (int)missle.Y))
                            {
                                tank.IsActive = false;
                                missle.IsActive = false;
                            }
                        }
                    }
                    _missles.RemoveAll(m => !m.IsActive);
                    _tanks.RemoveAll(t => !t.IsActive);
                    foreach (var handler in _handlers)
                    {
                        handler.Draw(_tanks, _missles, _arena);
                    }
                    Thread.Sleep(SLEEP_DURATION);

                    if (WinCondition())
                    {
                        foreach (var tank in _tanks)
                        {
                            var target = Scores.FirstOrDefault(s => s.ControllerName == tank.ControllerName);
                            if (target == null)
                                Scores.Add(new ScoreInfo { ControllerName = tank.ControllerName, Wins = 1 });
                            else
                                target.Wins++;
                        }
                        _newGame = true;
                    }
                }
            }
        }

        

        private void SetupNewGame()
        {
            _missles.Clear();
            _tanks.Clear();
            _newGame = false;
            AddTanks();
        }

        

        private void MoveMissle(Missile missle)
        {
            if (_arena.Boundary.Contains(new System.Drawing.PointF((float)missle.X, (float)missle.Y)))
            {
                missle.X += (float)Math.Sin(missle.Angle) * MISSLE_SPEED;
                missle.Y += (float)Math.Cos(missle.Angle) * MISSLE_SPEED;
            }
            else
            {
                missle.IsActive = false;
            }
        }

        private void AddTanks()
        {
            //for (var i = 0; i < Game.NUM_TANKS; i++)
            //{
            //    var AI = new StandStill();
            //    NewTank(AI);
            //}
            NewTank(new StandStill());
            NewTank(new RandomAI());
        }

        private static void NewTank(ITankController AI)
        {
            _tanks.Add(new Tank(AI)
            {
                Angle = 0,
                B = _random.Next(0, 255),
                G = _random.Next(0, 255),
                R = _random.Next(0, 255),
                Size = Game.TANK_SIZE,
                TurretAngle = 0,
                X = _random.Next((int)_arena.Border.X + 20, (int)_arena.Border.Width - 20),
                Y = _random.Next((int)_arena.Border.Y + 20, (int)_arena.Border.Height - 20),
            });
        }

        private void MoveTank(Tank tank)
        {
            var missle = tank.Move();
            if (missle != null)
                _missles.Add(missle);
        }


    }
}
