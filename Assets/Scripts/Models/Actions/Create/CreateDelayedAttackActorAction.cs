using System.Collections.Generic;
using Models.Actors;

namespace Models.Actions
{
    public class CreateDelayedAttackActorAction : SimpleAction
    {
        public const string ACTION_NAME = "CreateDelayedAttackActor";
        
        private IRoomActor _source;
        private IRoomActor _target;
        private int _timeToLive;
        private int _damage;

        public CreateDelayedAttackActorAction(IRoomActor source, IRoomActor target, int timeToLive, int damage)
        {
            ActionName = ACTION_NAME;
            _timeToLive = timeToLive;
            _source = source;
            _target = target;
            _damage = damage; 
        }
        
        public override IEnumerable<string> Execute(IRoom room)
        {
            room.Entities.Add(new DelayedAttackActor(_source, _target, _timeToLive, _damage));
            
            return new List<string>() { "You fire a hellfire missle." };
        }
    }
}