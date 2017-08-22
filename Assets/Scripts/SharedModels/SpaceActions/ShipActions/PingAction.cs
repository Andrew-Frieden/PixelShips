using PixelSpace.Models.SharedModels.SpaceUpdates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceActions
{
    //  dev action that notifies everything in the room
    public class PingAction : ShipAction
    {
        public override IEnumerable<SpaceAction> Execute()
        {
            //  ping should just add an update to all ship feeds in the room
            //var shipIds = State.Ships.Where(s => s.Room == this.SourceRoom).Select(ship => ship.Id);
            //var feeds = State.Feeds.Where(f => shipIds.Contains(f.Id));

            //foreach (var feed in feeds)
            //{
            //    var nextUpdate = new FeedUpdate
            //    {
            //        Text = string.Format("Detected a ping by {0}", SourceShip.Name),
            //        UniverseTime = DateTime.UtcNow,
            //        Id = Guid.NewGuid().ToString()
            //    };
            //    feed.Updates.Add(nextUpdate);
            //}

            return Enumerable.Empty<SpaceAction>();
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

        public PingAction(ISpaceState state, SpaceActionDbi dbi) : base(state, dbi) { }
    }
}
