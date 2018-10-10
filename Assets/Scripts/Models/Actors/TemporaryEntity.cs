using Models.Actions;
using Models.Stats;
using UnityEngine;

namespace Models.Actors
{
    public abstract class TemporaryEntity : FlexEntity
    {
        protected TemporaryEntity() : base()
        {
        }

        public override IRoomAction CleanupStep(IRoom room)
        {
            if (Stats[StatKeys.TimeToLive] == 0)
            {
                IsDestroyed = true;
            }

            return new DoNothingAction(this);
        }
    }
}