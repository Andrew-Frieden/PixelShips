using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.Ships
{
    public class ShipDto
    {
        public string Id { get; set; }
        public int? Version { get; set; }

        public string Name { get; set; }
        public int Hull { get; set; }
        public int MaxHull { get; set; }
        public DateTime LastJump { get; set; }

        public string RoomId { get; set; }
    }
}
