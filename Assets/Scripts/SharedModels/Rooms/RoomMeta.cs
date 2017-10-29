using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelSpace.Models.SharedModels.Rooms
{
    public class RoomMeta
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public IEnumerable<string> ExitIds { get; set; }
    }

    public static class RoomHelpers
    {
        public static RoomMeta GetMeta(this Room room)
        {
            return new RoomMeta
            {
                Id = room.Id,
                Description = room.Description,
                X = room.X,
                Y = room.Y,
                ExitIds = room.ExitIds
            };
        }
    }
}
