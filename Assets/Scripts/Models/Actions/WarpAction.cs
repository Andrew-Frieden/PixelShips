using System.Collections.Generic;
using Models.Factories;
using Controller;

namespace Models.Actions
{
    public class WarpAction : SimpleAction
    {
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