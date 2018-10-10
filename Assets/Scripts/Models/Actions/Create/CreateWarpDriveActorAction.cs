using System.Collections.Generic;
using Models.Actors;

namespace Models.Actions
{
    public class CreateWarpDriveActorAction : SimpleAction
    {
        public CreateWarpDriveActorAction(int timeToLive)
        {
            TimeToLive = timeToLive;
        }
        
        public override IEnumerable<TagString> Execute(IRoom room)
        {
            room.Entities.Add(new WarpDriveActor(TimeToLive));
            
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