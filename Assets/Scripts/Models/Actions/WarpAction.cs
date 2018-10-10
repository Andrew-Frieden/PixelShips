using System.Collections.Generic;
using Models.Factories;
using Controller;

namespace Models.Actions
{
    public class WarpAction : SimpleAction
    {
        //  bring on the hacks
        //  instead of figuring out how to serialize a RoomTemplate as part of a SimpleAction
        //  or build a non-SimpleAction,
        //  just stuff all the RoomTemplate properties 
        private const string ActorFlavor1 = "ActorFlavor1";
        private const string ActorFlavor2 = "ActorFlavor2";
        private const string ActorFlavor3 = "ActorFlavor3";
        private const string Difficulty = "ActorFlavor3";
        private const string RoomFlavor = "RoomFlavor";

        private readonly RoomTemplate _template;
        
        public WarpAction(RoomTemplate template)
        {
            _template = template;
        }
        
        public override IEnumerable<TagString> Execute(IRoom room)
        {
            room.PlayerShip.WarpDriveReady = false;
            room.PlayerShip.WarpTarget = _template;
            
            return new List<TagString>()
            {
                new TagString()
                {
                    Text = "You begin to jump into hyperspace!",
                    Tags = new List<EventTag> { }
                },
                new TagString()
                {
                    Text = "3...",
                    Tags = new List<EventTag> { }
                },
                new TagString()
                {
                    Text = "2...",
                    Tags = new List<EventTag> { }
                },
                new TagString()
                {
                    Text = "1...",
                    Tags = new List<EventTag> { }
                }
            };
        }
    }
}