using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Html5TankGame.Contracts
{
    public class GameElement
    {
        public GameElement()
        {
            IsActive = true;
        }

        public bool IsActive { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Angle { get; set; }
        
        public int R { get; set; }
        public int B { get; set; }
        public int G { get; set; }

        public int Size { get; set; }
    }
}