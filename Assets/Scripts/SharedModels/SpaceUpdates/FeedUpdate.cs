using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceUpdates
{
    public class FeedUpdate
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime UniverseTime { get; set; }
    }
}
