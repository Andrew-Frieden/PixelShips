using PixelSpace.Models.SharedModels.SpaceUpdates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceActions
{
    //  dev action that notifies everything in the room
    public class PingAction : ShipAction
    {
        public override IEnumerable<SpaceActionDbi> Execute()
        {
            //  create a notification for the source
            //  and create a notification for everyone else in the room
            var roomNotification = new FeedUpdate()
            {
                Id = Guid.NewGuid().ToString(),
                UniverseTime = DateTime.UtcNow,
                Text = string.Format("Detected a sector ping by [{0}]", this.SourceShip.Id),
                HiddenIds = new List<string> { this.SourceShip.Id },
                VisibleIds = new List<string>()
            };
            this.SourceRoom.Notifications.Add(roomNotification);

            if (this.SourceShip.IsUser)
            {
                var sourceNotification = new FeedUpdate()
                {
                    Id = Guid.NewGuid().ToString(),
                    UniverseTime = DateTime.UtcNow,
                    Text = "You sent a sector ping.",
                    HiddenIds = new List<string> (),
                    VisibleIds = new List<string> { this.SourceShip.Id }
                };

                this.SourceRoom.Notifications.Add(sourceNotification);
            }

            return Enumerable.Empty<SpaceActionDbi>();
        }

        public override bool Validate()
        {
            return true;
        }

        public override SpaceActionDbi ToDbi()
        {
            return new SpaceActionDbi
            {
                DateCreated = this.DateCreated,
                Name = this.Name,
                SourceId = this.SourceShip.Id,
                SourceRoomId = this.SourceRoom.Id,
                TargetId = null,
                SourceType = "ship",
                Id = this.Id,
                Version = this.Version
            };
        }

        public override string Name { get { return "ping"; } }

        public PingAction(IRoomState state, SpaceActionDbi dbi) : base(state, dbi) { }
    }
}
