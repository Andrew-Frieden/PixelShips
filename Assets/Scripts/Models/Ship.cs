using System;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu]
    public class Ship : ScriptableObject, IShip
    {
        public int Hull { get; set; }
        public int Gathering { get; set; }
        public int Transport { get; set; }
        public int Intelligence { get; set; }
        public int Combat { get; set; }
        public int Speed { get; set; }
        
        public Ship(int hull, int gathing, int transport, int intelligence, int combat, int speed)
        {
            Hull = hull;
            Gathering = gathing;
            Transport = transport;
            Intelligence = intelligence;
            Combat = combat;
            Speed = speed;
        }
    } 
}

