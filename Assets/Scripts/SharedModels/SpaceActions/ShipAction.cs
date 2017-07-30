using PixelSpace.Models.SharedModels.Ships;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels
{
    public abstract class ShipAction : SpaceAction
    {
        public Ship SourceShip { get; set; }

        protected override void BuildFromContext(SpaceActionDto dto)
        {
            base.BuildFromContext(dto);
            SourceShip = Context.Ships.Single(s => s.Id == dto.TargetId);
        }

        public ShipAction(SpaceContext context, SpaceActionDto dto) : base(context, dto) { }
        protected ShipAction() { }
    }
}
