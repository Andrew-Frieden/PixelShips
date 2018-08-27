using System;
using Actions;
using Models.Actions;

namespace Models.Actors
{
    public class DelayedAttackActor : DelayedActor
    {
        private readonly ICombatEntity _source;
        private readonly ICombatEntity _target;
        private readonly int _damage;

        public DelayedAttackActor(ICombatEntity source, ICombatEntity target, int timeToLive, int damage)
        {
            Id = Guid.NewGuid().ToString();
            Stats[TimeToLiveKey] = timeToLive;
            _source = source;
            _target = target;
            _damage = damage; 
        }

        public IRoomAction GetNextAction(IRoom s)
        {
            if (Stats[DelayedActor.TimeToLiveKey] == 1)
            {
                return new AttackAction(_source, _target, _damage);
            }
            else
            {
                return new DelayedAction($"An attack will hit {_target.GetLinkText()} in {Stats[DelayedActor.TimeToLiveKey]} ticks.");
            }
        }
    }
}