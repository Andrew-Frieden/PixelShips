﻿using System;

namespace Models
{
    public class GameState
    {
        public DateTime CurrentTime;

        public Expedition CurrentExpedition;
        public Homeworld Home;
        
        public int GetTicks()
        {
            return CurrentExpedition.Ticks;
        }
        
        public void Tick()
        {
            CurrentExpedition.Ticks++;
        }
    }
    
    public class Expedition
    {
        public int Ticks;
        public int Jumps;
        public Mission CurrentMission;
        public CommandShip CmdShip;
        public Room Room;
    }
    
    public class Homeworld
    {
        public string PlanetName;
        public int HardestMonsterSlainScore;
        public int DeepestExpedition;
    }
    
    public class Mission
    {
        public int MissionLevel;
    }
}