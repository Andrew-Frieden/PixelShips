using System.Collections.Generic;
using Models.Factories;
using Controller;

namespace Models.Actions
{
    public class WarpAction : IRoomAction
    {
        private readonly RoomTemplate _template;
        
        public WarpAction(RoomTemplate template)
        {
            _template = template;
        }
        
        public IEnumerable<StringTagContainer> Execute(IRoom room)
        {
            room.PlayerShip.WarpDriveReady = false;
            room.PlayerShip.WarpTarget = _template;
            
            return new List<StringTagContainer>()
            {
                new StringTagContainer()
                {
                    Text = "You begin to jump into hyperspace!",
                    ResultTags = new List<ActionResultTags> { }
                },
                new StringTagContainer()
                {
                    Text = "3...",
                    ResultTags = new List<ActionResultTags> { }
                },
                new StringTagContainer()
                {
                    Text = "2...",
                    ResultTags = new List<ActionResultTags> { }
                },
                new StringTagContainer()
                {
                    Text = "1...",
                    ResultTags = new List<ActionResultTags> { }
                }
            };
        }
    }
}