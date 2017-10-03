using PixelSpace.Models.SharedModels.Ships;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelSpace.Models.SharedModels
{
    public class ShipState
    {
        public Ship Ship { get; set; }
		public Room Room { get; set; }
        public IEnumerable<SpaceActionDbi> PossibleActions { get; set; }
        //public ISpaceState SpaceState { get; set; }

        public void ToDto()
        {
            //  drop circular references before serializing
            Ship.Room = null;

            foreach (var ship in Room.Ships)
            {
                ship.Room = null;
            }
        }
    }
}
