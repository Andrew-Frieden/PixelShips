using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceUpdates
{
    public class ShipFeed
    {
        public int? Version { get; set; }
        public string Id { get; set; }
        public List<ShipUpdate> Updates { get; set; }
    }

    public static partial class ShipFeedHelpers
    {
        public static ShipFeedDto ToDto(this ShipFeed feed)
        {
            return new ShipFeedDto
            {
                Version = feed.Version,
                Id = feed.Id,
                Updates = feed.Updates.Select(u => u.ToDto()).ToList()
            };
        }

        public static ShipFeed FromDto(this ShipFeedDto dto)
        {
            return new ShipFeed
            {
                Version = dto.Version,
                Id = dto.Id,
                Updates = dto.Updates.Select(u => u.FromDto()).ToList()
            };
        }
    }
}
