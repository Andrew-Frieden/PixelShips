using Amazon.DynamoDBv2.DataModel;
using PixelSpace.Models.SharedModels.Ships;
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

        public int Zone { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Description { get; set; }

        public List<string> ExitIds { get; set; }
        public List<string> ShipIds { get; set; }

        //[DynamoDBIgnore]
        //public List<Room> Exits { get; set; }

        [DynamoDBIgnore]
        public List<Ship> Ships { get; set; }

        [DynamoDBIgnore]
        public bool Modified { get; set; }  //  only write models that have been modified

        public void FromModel()
        {
            //ExitIds = Exits.Select(exit => exit.Id).ToList();
            ShipIds = Ships.Select(ship => ship.Id).ToList();
        }
    }
}
