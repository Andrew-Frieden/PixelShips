using PixelSpace.Models.SharedModels.SpaceActions;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using System;
using System.Collections.Generic;
using System.Linq;
using PixelSpace.Models.SharedModels.Ships;

namespace PixelSpace.Models.SharedModels
{
    public class RoomState : IRoomState
    {
        public Room Room { get; set; }
        public IEnumerable<Ship> Ships { get; set; }

        public RoomState(Room room, IEnumerable<Ship> ships)
        {
            Room = room;
            Ships = ships;
        }

        public RoomState(IRoomState state)
        {
            Room = state.Room;
            Ships = state.Ships;
        }
    }
}
