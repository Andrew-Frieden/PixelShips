using Newtonsoft.Json;
using PixelSpace.Models.SharedModels.Rooms;
using PixelSpace.Models.SharedModels.Ships;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelSpace.Models.SharedModels
{
    public class ShipState : IRoomState
    {
        public Room Room { get; set; }
        public IEnumerable<Ship> Ships { get; set; }

        [JsonIgnore]
        public Ship Ship { get { return Ships.Single(s => s.Id == ShipId); } }
        public string ShipId { get; set; }
        //public IEnumerable<RoomMeta> Map { get; set; }

        public ShipState(string shipId, IRoomState state)
        {
            ShipId = shipId;
            Room = state.Room;
            Ships = state.Ships;
        }

        public ShipState(string shipId, Room room, IEnumerable<Ship> ships)
        {
            ShipId = shipId;
            Room = room;
            Ships = ships;
        }

        public ShipState() { }

        //public IEnumerable<SpaceActionDbi> PossibleActions { get; set; }
        //public ISpaceState SpaceState { get; set; }
    }
}
