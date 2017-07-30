using PixelSpace.Models.SharedModels.SpaceUpdates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceActions
{
    //  dev action that notifies everything in the room
    public class PingAction : ShipAction
    {
        public override void Execute()
        {
            //  ping should just add an update to all ship feeds in the room
            var shipIds = Context.Ships.Where(s => s.Room == this.SourceRoom).Select(ship => ship.Id);
            var feeds = Context.ShipFeeds.Where(f => shipIds.Contains(f.Id));

            foreach (var feed in feeds)
            {
                var nextUpdate = new ShipUpdate
                {
                    Text = string.Format("Detected a ping by {0}", SourceShip.Name),
                    UniverseTime = DateTime.UtcNow,
                    UpdateId = Guid.NewGuid().ToString()
                };
                feed.Updates.Add(nextUpdate);
            }
        }

        public override bool Validate()
        {
            return true;
        }

        public override SpaceActionDto ToDto()
        {
            return new SpaceActionDto
            {
                DateCreated = this.DateCreated,
                Name = this.Name,
                SourceId = this.SourceShip.Id,
                SourceRoomId = this.SourceRoom.Id,
                TargetId = string.Empty,
                SourceType = "ship"
            };
        }

        public override string Name { get { return "ping"; } }

        public PingAction(SpaceContext context, SpaceActionDto dto) : base(context, dto) { }
    }
}
