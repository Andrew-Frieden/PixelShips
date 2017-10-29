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

            //if (State.Ships == null)
            //    throw new Exception("ship collection empty!");

            //var test = State.Ships.Where(s => s.Id == dbi.SourceId);

            //if (test.Count() < 1)
            //    throw new Exception("didn't find the ship!");
            try
            {
                SourceShip = State.Ships.Single(s => s.Id == dbi.SourceId);
            }
            catch (Exception e)
            {
                throw new Exception("ShipAction counldn't build from context: " + e.Message);
            }
        }

        public ShipAction(IRoomState state, SpaceActionDbi dbi) : base(state, dbi) { }
        protected ShipAction() { }
    }
}
