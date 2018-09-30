using System;
using System.Collections.Generic;
using Models.Actions;

namespace Models.Actors
{
    public class WarpDriveActor : TemporaryEntity
    {
        private enum DriveState
        {
            Cold = 0,
            WarmingUp = 1,
            Ready = 2
        }
        
        //TODO: This probably shouldn't be printed so lets remove
        private readonly Dictionary<DriveState, string> _lookText = new Dictionary<DriveState, string>
        {
            { DriveState.Cold, "Your warp drive is sitting there cold." },
            { DriveState.WarmingUp, "Your warp drive begins to resonate with power." },
            { DriveState.Ready, "Your warp drive is ready to jump." }
        };
        
        public WarpDriveActor(int timeToLive) : base()
        {
            IsHidden = true;
            Stats[TimeToLiveKey] = timeToLive;
        }

        public override void CalculateDialogue(IRoom room)
        {
        }

        public override TagString GetLookText()
        {
            return new TagString()
            {
                Text = _lookText[(DriveState) CurrentState]
            };
        }

        public override IRoomAction MainAction(IRoom s)
        {
            if (Stats[TimeToLiveKey] == 1)
            {
                return new WarpDriveReadyAction(s.PlayerShip);
            }
            
            return new DelayedAction(Id);
        }
    }
}