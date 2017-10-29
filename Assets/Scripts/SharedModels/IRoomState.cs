using PixelSpace.Models.SharedModels.Ships;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using System.Collections.Generic;

namespace PixelSpace.Models.SharedModels
{
    public interface IRoomState
    {
        Room Room { get; }
        IEnumerable<Ship> Ships { get; }
    }
}
