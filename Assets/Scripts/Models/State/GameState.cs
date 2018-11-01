using System;
using TextSpace.Framework;

namespace TextSpace.Models
{
    public class GameState : IExpeditionProvider, IHomeworldProvider
    {
        public DateTime CurrentTime { get; set; }
        public Expedition Expedition { get; set; }
        public Homeworld Home { get ; set; }

        public int Ticks => Expedition.Ticks;
        
        public void Tick()
        {
            Expedition.Ticks++;
        }
    }

    public interface IExpeditionProvider : IResolvableService
    {
        Expedition Expedition { get; }
    }

    public interface IHomeworldProvider : IResolvableService
    {
        Homeworld Home { get; }
    }
    
    public class Expedition
    {
        public int Ticks;
        public int Jumps;
        public Mission CurrentMission;
        public CommandShip CmdShip;
        public Room Room;

        public Expedition() { }

        public Expedition(CommandShip ship, IRoom room)
        {
            CmdShip = ship;
            Room = (Room)room;
            Room.SetPlayerShip(CmdShip);
        }
    }
    
    public class Homeworld
    {
        public string PlanetName;
        public string Description;
        public int HardestMonsterSlainScore;
        public int DeepestExpedition;
        public int ExpeditionCount;
    }
    
    public class Mission
    {
        public int MissionLevel;
        public int JumpsLeft;

    }
}