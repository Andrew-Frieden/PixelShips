using Amazon.DynamoDBv2.DataModel;
using Newtonsoft.Json;
using PixelSpace.Models.SharedModels.Ships;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels
{
    [DynamoDBTable("Rooms")]
    public class Room
    {
        [DynamoDBVersion]
        public int? Version { get; set; }
        [DynamoDBHashKey]
        public string Id { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public string Description { get; set; }

        public List<string> ExitIds { get; set; }
        public List<string> ShipIds { get; set; }

        public List<FeedUpdate> Notifications { get; set; }

        //[DynamoDBIgnore]
        //public List<Ship> Ships { get; set; }

        [DynamoDBIgnore]
        [JsonIgnore]
        public bool IsModified { get; set; }  //  only write models that have been modified
    }
}
