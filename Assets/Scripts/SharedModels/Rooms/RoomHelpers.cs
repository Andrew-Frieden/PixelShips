using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels
{
    public static partial class RoomHelpers
    {
        public static RoomDto ToDto(this Room room)
        {
            return new RoomDto()
            {
                Version = room.Version,
                Id = room.Id,
                LastResolved = room.LastResolved,
                Description = room.Description,
                X = room.X,
                Y = room.Y,
                Exits = room.Exits.Select(exit => exit.Id).ToList(),
                Ships = room.Ships.Select(s => s.Id).ToList()
            };
        }
    }
}
