using System;
using TextSpace.Framework;

namespace TextSpace.Models
{
    public class GameState : IRoomProvider, IShipProvider, IHomeworldProvider
    {
        public DateTime CurrentTime { get; set; }
        public Expedition Expedition { get; set; }
        public Homeworld Home { get ; set; }

        public Room Room
        {
            get { return Expedition.Room; }
            set { Expedition.Room = value; }
        }

        public CommandShip Ship => Expedition.CmdShip;
        public int Ticks => Expedition.Ticks;
        
        public void Tick()
        {
            Expedition.Ticks++;
        }
    }

    public interface IHomeworldProvider : IResolvableService
    {
        //  TODO refactor setter here
        Homeworld Home { get; set; }
    }

    public interface IRoomProvider : IResolvableService
    {   
        //  TODO refactor setter here
        Room Room { get; set; }
    }

    public interface IShipProvider : IResolvableService
    {
        CommandShip Ship { get; }
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