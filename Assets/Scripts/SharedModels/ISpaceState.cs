using PixelSpace.Models.SharedModels.Ships;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using System.Collections.Generic;

namespace PixelSpace.Models.SharedModels
{
    public interface ISpaceState
    {
        IEnumerable<Room> Rooms { get; set; }
        IEnumerable<Ship> Ships { get; set; }
        IEnumerable<SpaceAction> Actions { get; set; }
        IEnumerable<ShipFeed> Feeds { get; set; }

        IDictionary<string, Ship> ShipMap { get; set; }
        IDictionary<string, Room> RoomMap { get; set; }
        IDictionary<string, ShipFeed> FeedMap { get; set; }
    }
}
