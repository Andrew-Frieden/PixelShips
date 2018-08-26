using System.Collections.Generic;
using Models.Actors;

namespace Models.Actions
{
    public class CreateDelayedAttackAction : SimpleAction
    {
        private int _timeToLive;
        private ICombatEntity _source;
        private ICombatEntity _target;
        private int _damage;
        
        public override IEnumerable<string> Execute(IRoom room)
        {
            var actor = new DelayedAttackActor(_timeToLive, _source, _target, _damage);

            var sourceActor = room.FindRoomActorByGuid(_source.Id);
        }
    }
}