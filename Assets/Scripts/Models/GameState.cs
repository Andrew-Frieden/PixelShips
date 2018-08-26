using System;
using UnityEngine;

namespace Models
{
    public class GameState
    {
        public DateTime CurrentTime;
        public Room Room;

        public void LogData()
        {
            Debug.Log("test");
        }
    }
}