//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Html5TankGame.Contracts;
//using SignalR;

//namespace Html5TankGame
//{
//    public class NewPlayerHandler : PersistentConnection
//    {
//        protected override System.Threading.Tasks.Task OnReceivedAsync(string clientId, string data)
//        {
//            int colourIndex = Game.NumberOfShips;
//            if (Game.NumberOfShips > _colours.Length - 1)
//                colourIndex = Game.NumberOfShips % _colours.Length;

//            var colour = _colours[colourIndex];
//            var tank = new Tank() { Colour = colour, Name = data, X = 50, Y = 50 };
//            Game.AddTank(tank);
//            return Connection.Broadcast(tank);
//        }


//        private readonly string[] _colours = new string[]
//                                            {
//                                                "red",
//                                                "white",
//                                                "blue",
//                                                "yellow"
//                                            };
//    }
//}