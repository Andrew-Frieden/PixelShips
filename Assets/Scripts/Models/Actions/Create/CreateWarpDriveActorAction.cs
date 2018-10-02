using System.Collections.Generic;
using Models.Actors;

namespace Models.Actions
{
    public class CreateWarpDriveActorAction : SimpleAction
    {
        public const string ACTION_NAME = "CreateWarpDriveActor";
        
        private int _timeToLive;

        public CreateWarpDriveActorAction(int timeToLive)
        {
            ActionName = ACTION_NAME;
            
            _timeToLive = timeToLive;
        }
        
        public override IEnumerable<TagString> Execute(IRoom room)
        {
            room.Entities.Add(new WarpDriveActor(_timeToLive));
            
            return new List<TagString>()
            {
                new TagString()
                {
                    Text = "Your warp drive begins charging."
                }
            };
        }
    }
}