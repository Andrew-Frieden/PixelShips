using TextSpace.Models.Actions;
using TextSpace.Models.Dtos;
using TextSpace.Models.Stats;
using UnityEngine;

namespace TextSpace.Models.Actors
{
    public abstract class TemporaryEntity : FlexEntity
    {
        public TemporaryEntity(FlexEntityDto dto) : base(dto) { }

        public TemporaryEntity(FlexData data) : base(data) { }

        protected TemporaryEntity() { }

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