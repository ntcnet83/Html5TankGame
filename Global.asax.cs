using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using SignalR;
//using SignalR.Hosting.AspNet.Routing;
//using SignalR.Web.Routing;

namespace Html5TankGame
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //RouteTable.Routes.MapConnection<NewPlayerHandler>("playerconnection", "playerconnection/{*operation}");
            //RouteTable.Routes.MapConnection<KeyPressHandler>("keypressconnection", "keypressconnection/{*operation}");
            RouteTable.Routes.MapConnection<ClientHandler>("game", "game/{*operation}");
            Game.StartGame();
        }
    }
}