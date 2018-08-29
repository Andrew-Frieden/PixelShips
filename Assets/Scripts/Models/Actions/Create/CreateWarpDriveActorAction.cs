using System.Collections.Generic;
using Models.Actors;

namespace Models.Actions
{
    public class CreateWarpDriveActorAction : SimpleAction
    {
        public const string ACTION_NAME = "CreateWarpDriveActor";
        
        private ICombatEntity _source;
        private int _timeToLive;

        public CreateWarpDriveActorAction(ICombatEntity source, int timeToLive)
        {
            ActionName = ACTION_NAME;
            _timeToLive = timeToLive;
            _source = source;
        }
        
        public override IEnumerable<string> Execute(IRoom room)
        {
            room.Entities.Add(new WarpDriveActor(_source, _timeToLive));
            
            return new List<string>() { "You begin to spin up your warp drive." };
        }
    }
}