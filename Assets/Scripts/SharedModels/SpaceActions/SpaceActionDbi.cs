using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels
{
    [DynamoDBTable("VerseActions")]
    public class SpaceActionDbi
    {
        [DynamoDBVersion]
        public int? Version { get; set; }
        [DynamoDBHashKey]
        public string Id { get; set; }
        //[DynamoDBRangeKey]
        public DateTime DateCreated { get; set; }

        public string Name { get; set; }            //  attack, move, use, inspect
        public string SourceRoomId { get; set; }
        public string SourceType { get; set; }      //  'ship', 'widget', 'hazard'
        public string SourceId { get; set; }
        public string TargetId { get; set; }
    }
}
