using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceUpdates
{
    public class ShipFeedDto
    {
        public int? Version { get; set; }
        public string Id { get; set; }
        public List<ShipUpdateDto> Updates { get; set; }
    }
}
