using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceActions
{
    public class DelayedJumpAction : ShipAction
    {
        public Room TargetRoom { get; set; }

        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void Execute()
        {
            // add notifications to source room and target room
            // move ship to target room

            //create a jump action and drop it in the list for +X seconds
            var delayedJump = new SpaceActionDto
            {
                //DateActivated = DateTime.UtcNow + 5
                SourceId = this.SourceShip.Id,
                DateCreated = DateTime.UtcNow,
                Name = "jump",
                SourceType = "ship",
                SourceRoomId = this.SourceRoom.Id,
                TargetId = TargetRoom.Id
            };
            
            

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
            throw new NotImplementedException();
        }

        public DelayedJumpAction(SpaceContext context, SpaceActionDto dto) : base(context, dto) { }
    }
}
