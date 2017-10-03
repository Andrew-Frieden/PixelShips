using PixelSpace.Models.SharedModels.Ships;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using System.Collections.Generic;

namespace PixelSpace.Models.SharedModels
{
    public interface ISpaceState
    {
        IEnumerable<Room> Rooms { get; set; }
        IEnumerable<Ship> Ships { get; set; }
    }
}
