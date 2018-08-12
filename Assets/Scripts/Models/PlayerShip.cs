using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Models
{
    public class PlayerShip : Ship
    {
        public PlayerShip(int hull, int gathing, int transport, int intelligence, int combat, int speed) : base(hull, gathing, transport, intelligence, combat, speed) { }
    }
}
