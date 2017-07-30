using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceActions
{
    public class SpaceActor
    {
        public int? Version { get; set; }
        public string Id { get; set; }
        public string ActorType { get; set; }   //  'ship', 'hazard'
        public List<SpaceAction> Actions { get; set; }
        public DateTime LastResolved { get; set; }
    }
}
