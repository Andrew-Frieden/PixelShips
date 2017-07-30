using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels
{
    public class JumpAction : ShipAction
    {
        public Room TargetRoom { get; set; }

        public override string Name
        {
            get
            {
                return "jump";
            }
        }

        public override void Execute()
        {
            // add notifications to source room and target room
            // move ship to target room
            throw new NotImplementedException();
        }

        public override bool Validate()
        {
            //  make sure target room is an exit of source room
            //  make sure source ship has enough power
            //  make sure source ship isn't on cooldown
            throw new NotImplementedException();
        }

        protected override void BuildFromContext(SpaceActionDto dto)
        {
            base.BuildFromContext(dto);
            TargetRoom = Context.Rooms.Single(r => r.Id == dto.TargetId);
        }

        public override SpaceActionDto ToDto()
        {
            return new SpaceActionDto
            {
                DateCreated = this.DateCreated,
                Name = this.Name,
                SourceId = this.SourceShip.Id,
                SourceRoomId = this.SourceRoom.Id,
                TargetId = this.TargetRoom.Id,
                SourceType = "ship"
            };
        }

        public JumpAction(SpaceContext context, SpaceActionDto dto) : base(context, dto) { }
        protected JumpAction() { }

    }
}
