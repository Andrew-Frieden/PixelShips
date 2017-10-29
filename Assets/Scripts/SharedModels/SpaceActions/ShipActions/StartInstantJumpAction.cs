using PixelSpace.Models.SharedModels.SpaceActions;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels
{
    public class StartInstantJumpAction : ShipAction
    {
        public string TargetRoomId { get; set; }

        public override string Name
        {
            get
            {
                return "start_instant_jump";
            }
        }

        public override IEnumerable<SpaceActionDbi> Execute()
        {
            //  remove the ship from the room
            SourceRoom.ShipIds.Remove(SourceShip.Id);
            SourceShip.RoomId = string.Empty;

            //  set the target jump room on the ship
            SourceShip.JumpRoomId = TargetRoomId;
            SourceShip.IsModified = true;

            SourceRoom.Notifications.AddRange(GetNotifications());
            return GetResultingActions();
        }

        private IEnumerable<FeedUpdate> GetNotifications()
        {
            var notes = new List<FeedUpdate>();

            var actionNote = FeedUpdate.New();
            actionNote.HiddenIds.Add(SourceShip.Id);
            actionNote.Text = string.Format("[{0}] warped out of the room.", this.SourceShip.Id);

            notes.Add(actionNote);

            if (SourceShip.IsUser)
            {
                var srcNote = FeedUpdate.New();
                srcNote.VisibleIds.Add(SourceShip.Id);
                srcNote.Text = "You warped out of the room.";
                notes.Add(srcNote);
            }
            return notes;
        }

        private IEnumerable<SpaceActionDbi> GetResultingActions()
        {
            var resultingActions = new List<SpaceActionDbi>();
            //var actionFactory = new SpaceActionFactory(this.State);

            var endJumpDbi = new SpaceActionDbi
            {
                Id = Guid.NewGuid().ToString(),
                DateCreated = this.DateCreated,
                Name = "end_instant_jump",
                SourceId = this.SourceShip.Id,
                SourceRoomId = this.TargetRoomId,
                TargetId = this.TargetRoomId,
                SourceType = "ship"
            };

            resultingActions.Add(endJumpDbi);

            return resultingActions;

            //var endJumpAction = actionFactory.GetModel(endJumpDbi);
            //resultingActions.Add(endJumpAction);
            //return resultingActions;
        }

        public override bool Validate()
        {
            //  make sure room contains the ship
            if (!SourceRoom.ShipIds.Contains(this.SourceShip.Id))
            {
                return false;
            }

            //  make sure room being jumped to is an exit of the current room
            if (!SourceRoom.ExitIds.Contains(TargetRoomId))
            {
                return false;
            }
            
            return true;
        }

        protected override void BuildFromContext(SpaceActionDbi dbi)
        {
            base.BuildFromContext(dbi);
            TargetRoomId = dbi.TargetId;
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
                TargetId = this.TargetRoomId,
                SourceType = "ship"
            };
        }

        public StartInstantJumpAction(IRoomState state, SpaceActionDbi dbi) : base(state, dbi) { }
    }
}
