using System;
using UnityEngine;

namespace Models
{
    public class GameState
    {
        public DateTime CurrentTime;
        public IRoom Room;
        public CommandShip CommandShip;

        public void LogData()
        {
            Debug.Log("test");
        }
    }
}