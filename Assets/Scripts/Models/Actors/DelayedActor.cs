using System.Collections.Generic;
using Models.Actions;

namespace Models.Actors
{
    public abstract class DelayedActor : FlexEntity
    {
        public const string TimeToLiveKey = "timetolive";

        protected DelayedActor()
        {
            Stats = new Dictionary<string, int>();
        }

        public override void AfterAction(IRoom room)
        {
            if (Stats[TimeToLiveKey] == 0)
            {
                room.Entities.Remove(this);
            }
        }
    }
}