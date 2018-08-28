using System.Collections.Generic;
using Models.Actors;

namespace Models.Actions
{
    public class CreateWarpDriveActorAction : SimpleAction
    {
        public const string ACTION_NAME = "CreateWarpDriveActor";
        
        private ICombatEntity _source;
        private ICombatEntity _target;
        private int _timeToLive;
        private int _damage;

        public CreateWarpDriveActorAction(ICombatEntity source, ICombatEntity target, int timeToLive, int damage)
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