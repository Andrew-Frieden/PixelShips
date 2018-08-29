using System.Collections.Generic;
using Models.Actors;

namespace Models.Actions
{
    public class CreateShieldActorAction : SimpleAction
    {
        public const string ACTION_NAME = "CreateShieldActor";
        
        private ICombatEntity _source;
        private ICombatEntity _target;
        private int _timeToLive;
        private int _damageReduction;

        public CreateShieldActorAction(ICombatEntity source, ICombatEntity target, int timeToLive, int damageReduction)
        {
            ActionName = ACTION_NAME;
            _timeToLive = timeToLive;
            _source = source;
            _target = target;
            _damageReduction = damageReduction; 
        }
        
        public override IEnumerable<string> Execute(IRoom room)
        {
            room.Entities.Add(new ShieldActor(_source, _target, _timeToLive, _damageReduction));
            
            return new List<string>() { "You generate a shield." };
        }
    }
}