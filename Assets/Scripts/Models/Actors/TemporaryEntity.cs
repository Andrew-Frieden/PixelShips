using Models.Actions;
using UnityEngine;

namespace Models.Actors
{
    public abstract class TemporaryEntity : FlexEntity
    {
        public const string TimeToLiveKey = "timetolive";

        protected TemporaryEntity() : base()
        {
        }

        public override IRoomAction CleanupStep(IRoom room)
        {
            if (Stats[TimeToLiveKey] == 0)
            {
                IsDestroyed = true;
            }

            return new DoNothingAction(this);
        }
    }
}