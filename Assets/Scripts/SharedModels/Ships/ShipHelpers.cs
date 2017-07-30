using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.Ships
{
    public static partial class ShipHelpers
    {
        public static ShipDto ToDto(this Ship ship)
        {
            return new ShipDto
            {
                Hull = ship.Hull,
                Id = ship.Id,
                Version = ship.Version,
                LastJump = ship.LastJump,
                Name = ship.Name,
                MaxHull = ship.MaxHull,
                RoomId = ship.Room.Id
            };
        }
    }
}
