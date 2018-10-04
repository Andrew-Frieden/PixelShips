using System.Collections.Generic;
using Models.Actions;
using Models.Dtos;

namespace Models.RoomEntities.Mobs.Kelp
{
    public abstract class Verdant : Mob
    {
        public override IRoomAction CleanupStep(IRoom room)
        {
            if (IsDestroyed)
            {
                return new DropGatherableAction(this, new ScrapGatherable("Kelp Fiber"));
            }
            return new DoNothingAction(this);
        }

        protected Verdant() : base() { }

        protected Verdant(FlexEntityDto dto, IRoom room) : base(dto, room) { }

        protected Verdant(Dictionary<string, int> stats, Dictionary<string, string> values) : base(stats, values) { }
    }
}