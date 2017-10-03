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

        public override IEnumerable<SpaceActionDbi> Execute()
        {
            // add notifications to source room and target room
            // move ship to target room
            throw new NotImplementedException();
        }

        public override bool Validate()
        {
            var startRoom = SourceRoom;

            //  make sure room being jumped to is an exit of the current room
            if (!SourceRoom.ExitIds.Contains(TargetRoom.Id))
            {
                return false;
            }
            
            //  check cooldown on jump

            //  make sure target room is an exit of source room
            //  make sure source ship has enough power
            //  make sure source ship isn't on cooldown
            throw new NotImplementedException();
        }

        protected override void BuildFromContext(SpaceActionDbi dbi)
        {
            base.BuildFromContext(dbi);
            TargetRoom = State.Rooms.Single(r => r.Id == dbi.TargetId);
        }

        public override SpaceActionDbi ToDbi()
        {
            return new SpaceActionDbi
            {
                DateCreated = this.DateCreated,
                Name = this.Name,
                SourceId = this.SourceShip.Id,
                SourceRoomId = this.SourceRoom.Id,
                TargetId = this.TargetRoom.Id,
                SourceType = "ship"
            };
        }

        public JumpAction(ISpaceState state, SpaceActionDbi dbi) : base(state, dbi) { }
        protected JumpAction() { }

    }
}
