using System;
using Models.Actions;
using Models.Stats;

namespace Models.Actors
{
    public class ShieldActor : TemporaryEntity
    {
        private readonly IRoomActor _source;
        private readonly IRoomActor _target;
        private readonly int _damageReduction;

        public ShieldActor(IRoomActor source, IRoomActor target, int timeToLive, int damageReduction) : base()
        {
            Stats[StatKeys.TimeToLive] = timeToLive;
            _source = source;
            _target = target;
            _damageReduction = damageReduction;
            IsHidden = true;
        }

        public override void CalculateDialogue(IRoom room)
        {
        }

        public override TagString GetLookText()
        {
            return new TagString();
        }

        public override IRoomAction MainAction(IRoom s)
        {
            return new DelayedAction(Id);
        }
    }
}