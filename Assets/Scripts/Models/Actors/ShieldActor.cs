using System;
using Models.Actions;

namespace Models.Actors
{
    public class ShieldActor : TemporaryEntity
    {
        private readonly IRoomActor _source;
        private readonly IRoomActor _target;
        private readonly int _damageReduction;

        public ShieldActor(IRoomActor source, IRoomActor target, int timeToLive, int damageReduction) : base()
        {
            Stats[TimeToLiveKey] = timeToLive;
            _source = source;
            _target = target;
            _damageReduction = damageReduction;
            IsHidden = true;
        }

        public override ABDialogueContent CalculateDialogue(IRoom room)
        {
            throw new NotImplementedException();
        }

        public override string GetLookText()
        {
            return "";
        }

        public override IRoomAction GetNextAction(IRoom s)
        {
            return new DelayedAction(Id);
        }
    }
}