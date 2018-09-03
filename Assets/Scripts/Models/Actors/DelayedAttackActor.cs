using System;
using Models.Actions;
using Unity.Collections.LowLevel.Unsafe;

namespace Models.Actors
{
    public class DelayedAttackActor : DelayedActor
    {
        private readonly IRoomActor _source;
        private readonly IRoomActor _target;
        private readonly int _damage;

        public DelayedAttackActor(IRoomActor source, IRoomActor target, int timeToLive, int damage) :base()
        {
            Id = Guid.NewGuid().ToString();
            Stats[TimeToLiveKey] = timeToLive;
            _source = source;
            _target = target;
            _damage = damage; 
        }

        public override string GetLookText()
        {
            return "";
        }

        public override string GetLinkText()
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