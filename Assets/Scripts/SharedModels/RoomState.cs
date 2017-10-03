using PixelSpace.Models.SharedModels.SpaceActions;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using System;
using System.Collections.Generic;
using System.Linq;
using PixelSpace.Models.SharedModels.Ships;

namespace PixelSpace.Models.SharedModels
{
    public class RoomState : ISpaceState
    {
        public Room Room { get { return Rooms.Single(); } }

        public IEnumerable<Room> Rooms { get; set; }
        public IEnumerable<Ship> Ships { get; set; }

        private SpaceActionFactory actionFactory;

        public RoomState(Room room, IEnumerable<Ship> ships)
        {
            //  setup the room and ship references

            //  setup the room model
            if (room.ShipIds == null)
                room.ShipIds = new List<string>();

            room.Ships = new List<Ship>();
            Rooms = new List<Room> { room };

            //  setup ships models and link with room
            Ships = ships;
            foreach (var ship in Ships)
            {
                if (room.ShipIds.Contains(ship.Id))
                {
                    room.Ships.Add(ship);
                    ship.Room = room;
                }
            }
        }


    }
}
