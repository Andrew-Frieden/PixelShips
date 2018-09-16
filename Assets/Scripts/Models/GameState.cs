using System;
using UnityEngine;

namespace Models
{
    public class GameState
    {
        public DateTime CurrentTime;
        public IRoom Room;
        public CommandShip CommandShip;
        private int _ticks;

        public int GetTicks()
        {
            return _ticks;
        }
        
        public void Tick()
        {
            _ticks++;
        }
    }
}