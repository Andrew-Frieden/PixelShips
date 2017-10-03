using PixelSpace.Models.SharedModels.Ships;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels
{
    public class EndInstantJumpAction : ShipAction
    {
        public Room TargetRoom { get; set; }

        public override string Name
        {
            get
            {
                return "end_instant_jump";
            }
        }

        public override IEnumerable<SpaceActionDbi> Execute()
        {
            // put the ship in the target room
            SourceRoom.Ships.Add(SourceShip);

            SourceShip.Room = SourceRoom;
            SourceShip.JumpRoomId = string.Empty;
            SourceShip.IsModified = true;

            // add notifications to source room and target room
            SourceRoom.Notifications.AddRange(GetNotifications());

            return Enumerable.Empty<SpaceActionDbi>();
        }

        private IEnumerable<FeedUpdate> GetNotifications()
        {
            var notes = new List<FeedUpdate>();

            var roomNotification = new FeedUpdate()
            {
                Id = Guid.NewGuid().ToString(),
                UniverseTime = DateTime.UtcNow,
                Text = string.Format("[{0}] translates into the sector.", this.SourceShip.Id),
                HiddenIds = new List<string> { this.SourceShip.Id },
                VisibleIds = new List<string>()
            };
            notes.Add(roomNotification);

            if (SourceShip.IsUser)
            {
                var sourceNotification = new FeedUpdate()
                {
                    Id = Guid.NewGuid().ToString(),
                    UniverseTime = DateTime.UtcNow,
                    Text = "Sector jump complete.",
                    HiddenIds = new List<string> (),
                    VisibleIds = new List<string> { this.SourceShip.Id }
                };

                notes.Add(sourceNotification);
            }

            return notes;
        }

        public override bool Validate()
        {
            //  make sure ship isn't already in the room...I guess?
            if (SourceRoom.Ships.Contains(this.SourceShip))
            {
                return false;
            }
            
            return true;
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
                Id = this.Id,
                DateCreated = this.DateCreated,
                Name = this.Name,
                SourceId = this.SourceShip.Id,
                SourceRoomId = this.SourceRoom.Id,
                TargetId = this.TargetRoom.Id,
                SourceType = "ship"
            };
        }

        public EndInstantJumpAction(ISpaceState state, SpaceActionDbi dbi) : base(state, dbi) { }
    }
}
