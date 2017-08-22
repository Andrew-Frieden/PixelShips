using Amazon.DynamoDBv2.DataModel;
using System;

namespace PixelSpace.Models.SharedModels.Ships
{
    [DynamoDBTable("Ships")]
    public class Ship
    {
        [DynamoDBVersion]
        public int? Version { get; set; }
        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Name { get; set; }
        public int Hull { get; set; }
        public int MaxHull { get; set; }
        public DateTime LastJump { get; set; }
        public string RoomId { get; set; }

        public bool IsUser { get; set; }

        [DynamoDBIgnore]
        public Room Room { get; set; }

        [DynamoDBIgnore]
        public bool Modified { get; set; }

        public static double JUMP_COOLDOWN = 5;
        public bool CanJump
        {
            get
            {
                return (DateTime.UtcNow - LastJump).TotalSeconds > JUMP_COOLDOWN;
            }
        }

        public void FromModel()
        {
            RoomId = Room.Id;
        }
    }
}
