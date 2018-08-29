using System;
using Models.Actions;

namespace Models.Actors
{
    public class WarpDriveActor : DelayedActor
    {
        private readonly ICombatEntity _source;
        
        public WarpDriveActor(ICombatEntity source, int timeToLive) :base()
        {
            Id = Guid.NewGuid().ToString();
            Stats[TimeToLiveKey] = timeToLive;
            _source = source;
        }
        
        public override string GetLookText()
        {
            throw new System.NotImplementedException();
        }

        public override string GetLinkText()
        {
            return "Warp drive";
        }

        public override IRoomAction GetNextAction(IRoom s)
        {
            if (Stats[TimeToLiveKey] == 1)
            {
                return new WarpAction();
            }
            else
            {
                return new DelayedAction($"Your warp drive is charging. It will be ready ", Id);
            }
        }
    }
}