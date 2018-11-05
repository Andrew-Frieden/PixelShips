using System;
using System.Collections.Generic;
using TextSpace.Framework;

namespace TextSpace.Models
{
    public class GameState : IRoomProvider, IShipProvider, IHomeworldProvider, IMissionProvider
    {
        public DateTime CurrentTime { get; set; }
        public Expedition Expedition { get; set; }
        public Homeworld Home { get ; set; }

        public Room Room
        {
            get { return Expedition.Room; }
            set { Expedition.Room = value; }
        }

        public Mission Mission
        {
            get
            {
                if (Expedition == null)
                    return null;
                return Expedition.CurrentMission;
            }
            set { Expedition.CurrentMission = value; }
        }

        public CommandShip Ship => Expedition.CmdShip;
        public int Ticks => Expedition.Ticks;
        public void Tick() { Expedition.Ticks++; }
    }

    public interface IHomeworldProvider : IResolvableService
    {
        //  TODO refactor setter
        Homeworld Home { get; set; }
    }

    public interface IRoomProvider : IResolvableService
    {
        //  TODO refactor setter
        Room Room { get; set; }
    }

    public interface IShipProvider : IResolvableService
    {
        CommandShip Ship { get; }
    }

    //  TODO refactor setter
    public interface IMissionProvider : IResolvableService
    {
        Mission Mission { get; set; }
    }

    ////  maybe something like this?
    //public interface IHomeworldShaper : IResolvableService
    //{
    //    void SetHomeworld(Homeworld world);
    //}

    //public interface IRoomShaper : IResolvableService
    //{
    //    void SetRoom(Room room);
    //}

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
        public RoomFlavor RoomFlavor;
        public MissionType MissionType;
        public IRoomActor Objective;
        public IEnumerable<IRoomActor> Entities;

        public int MissionLevel;
        public int MaximumMissionJumps;
        public int ZoneJumps;
    }

    public enum MissionType
    {
        Interact = 0,
        DestroyMob = 1
    }
}