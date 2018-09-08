using System;
using Models.Actions;

namespace Models.Actors
{
    public class DelayedAttackActor : TemporaryEntity
    {
        private readonly IRoomActor _source;
        private readonly IRoomActor _target;
        private readonly int _damage;

        public DelayedAttackActor(IRoomActor source, IRoomActor target, int timeToLive, int damage) : base()
        {
            Stats[TimeToLiveKey] = timeToLive;
            _source = source;
            _target = target;
            _damage = damage; 
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
            if (Stats[TimeToLiveKey] == 1)
            {
                return new AttackAction(_source, _target, _damage);
            }
            else
            {
                return new DelayedAction($"A hellfire missle will hit {_target.GetLinkText()} ", Id);
            }
        }
    }
}