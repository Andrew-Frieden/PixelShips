using System;
using Actions;
using Models.Actions;

namespace Models.Actors
{
    public class DelayedAttackActor : DelayedActor
    {
        private ICombatEntity _source;
        private ICombatEntity _target;
        private int _damage;

        public string Id { get; }
        public ABDialogueContent DialogueContent { get; }

        public DelayedAttackActor(int timeToLive, ICombatEntity source, ICombatEntity target, int damage)
        {
            Id = Guid.NewGuid().ToString();
            _timeToLive = timeToLive;
            _source = source;
            _target = target;
            _damage = damage; 
        }
        
        public string GetLookText()
        {
            throw new System.NotImplementedException();
        }

        public string GetLinkText()
        {
            throw new System.NotImplementedException();
        }

        public IRoomAction GetNextAction(IRoom s)
        {
            if (_timeToLive == 1)
            {
                return new AttackAction(_source, _target, _damage);
            }
            else
            {
                return new DelayedAction($"An attack will hit {_target.GetLinkText()} in {_timeToLive} ticks.");
            }
        }
    }
}