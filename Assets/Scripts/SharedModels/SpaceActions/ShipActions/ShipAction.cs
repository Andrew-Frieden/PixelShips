using PixelSpace.Models.SharedModels.Ships;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels
{
    public abstract class ShipAction : SpaceAction
    {
        public Ship SourceShip { get; set; }

        protected override void BuildFromContext(SpaceActionDbi dbi)
        {
            base.BuildFromContext(dbi);
            SourceShip = State.Ships.Single(s => s.Id == dbi.SourceId);
        }

        public ShipAction(ISpaceState state, SpaceActionDbi dbi) : base(state, dbi) { }
        protected ShipAction() { }
    }
}
