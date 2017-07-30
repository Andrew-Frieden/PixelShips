using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceUpdates
{
    //  [cmdr rex] begins spinning up his warp drive.
    //  [pirate ship] has launches a photon torpedo at you.
    //  [dark vessel] hit you with a plasma cannon for 14!
    //  [unknown] has just translated into the sector

    public class ShipUpdateDto
    {
        //public string ShipId { get; set; }
        public string UpdateId { get; set; }
        public string Text { get; set; }
        public DateTime UniverseTime { get; set; }
    }
}
