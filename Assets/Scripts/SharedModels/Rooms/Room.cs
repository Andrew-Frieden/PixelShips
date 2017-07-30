using PixelSpace.Models.SharedModels.Ships;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels
{
    public class Room
    {
        public int? Version { get; set; }
        public string Id { get; set; }
        public DateTime LastResolved { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public string Description { get; set; }
        public List<Room> Exits { get; set; }
        public List<Ship> Ships { get; set; }
    }
}
