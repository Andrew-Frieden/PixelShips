using Amazon.DynamoDBv2.DataModel;
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

        public List<string> VisibleIds { get; set; }
        public List<string> HiddenIds { get; set; }

        [DynamoDBIgnore]
        public bool IsGlobal
        {
        	get
        	{
                return (VisibleIds == null || VisibleIds.Count == 0)
                    && (HiddenIds == null || HiddenIds.Count == 0);
        	}
        }

        public static FeedUpdate New()
        {
            return new FeedUpdate()
            {
                Id = Guid.NewGuid().ToString(),
                UniverseTime = DateTime.UtcNow,
                VisibleIds = new List<string>(),
                HiddenIds = new List<string>()
            };
        }
    }
}
