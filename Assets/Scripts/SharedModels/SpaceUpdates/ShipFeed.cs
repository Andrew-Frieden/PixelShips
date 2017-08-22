using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceUpdates
{
    [DynamoDBTable("Feeds")]
    public class ShipFeed
    {
        [DynamoDBVersion]
        public int? Version { get; set; }
        [DynamoDBHashKey]
        public string Id { get; set; }
        [DynamoDBIgnore]
        public bool Modified { get; set; }
        public List<FeedUpdate> Updates { get; set; }   //  should actually have an AddUpdate() method that drops old ones
    }

    // everytime client reads, keep track of update ids that have been seen in last X minutes
    // everytime client reads, attempt to delete all seen updates. even if update fails, client will have seen feed items it has read recently
}
