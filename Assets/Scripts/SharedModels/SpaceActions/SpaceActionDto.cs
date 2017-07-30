using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels
{
    public class SpaceActionDto
    {
        public string Name { get; set; }            //  attack, move, use, inspect
        public string SourceRoomId { get; set; }
        public string SourceType { get; set; }      //  'ship', 'widget', 'hazard'
        public string SourceId { get; set; }
        public string TargetId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
