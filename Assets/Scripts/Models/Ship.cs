using System;
using UnityEngine;

namespace Models
{
    public class Ship : IShip
    {
        public int Hull { get; set; }
        public int Gathering { get; }
        public int Transport { get; }
        public int Intelligence { get; }
        public int Combat { get; }
        public int Speed { get; }
        
        public Ship(int gathing, int transport, int intelligence, int combat, int speed, int hull)
        {
            Gathering = gathing;
            Transport = transport;
            Intelligence = intelligence;
            Combat = combat;
            Speed = speed;
            Hull = hull;
        }
    } 
}

