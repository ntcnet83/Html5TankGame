using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Html5TankGame.Contracts;
using SignalR;

namespace Html5TankGame
{
    public class ClientHandler : PersistentConnection
    {
        protected override System.Threading.Tasks.Task OnConnectedAsync(IRequest request, string connectionId)
        {
            Game.AddGameHandler(this);
            return base.OnConnectedAsync(request, connectionId);
        }

        internal void Draw(List<Tank> tanks, List<Missile> missles, Arena arena)
        {
            string debug = string.Empty;
            Connection.Broadcast(new DrawInfo
            {
                Tanks = tanks.ToArray(),
                Missles = missles.ToArray(),
                Arena = arena,
                Debug = debug,
                Scores = Game.Scores.ToArray(),
            });
        }
    }
}