using System;
using System.Collections.Generic;
using Models.Text;
using PixelShips.Helpers;

namespace Models
{
    public class Room : IRoom, ITextEntity
    {
        private string Link { get; }
        
        public string Id { get; }
        public CommandShip PlayerShip { get; }
        public string Description { get; }
        public RoomFlavor Flavor { get; }
        public List<IRoomEntity> Entities { get; }

        public Room(string description, string link, CommandShip ship, List<IRoomEntity> roomEntities)
        {
            Id = Guid.NewGuid().ToString();
            Description = description;
            Link = link;
            PlayerShip = ship;
            Entities = roomEntities;
        }

        public List<string> ResolveNext(IRoomAction playerAction)
        {
            var actions = new List<IRoomAction>() {playerAction};
            Entities.ForEach(e => actions.Add(e.GetNextAction(this)));

            var resultText = new List<string>();
            actions.ForEach(a => resultText.AddRange(a.Execute(this)));

            return resultText;
        }

        public TextBlock GetLookText()
        {
            return new TextBlock(Description.GetDescriptionWithLink(Link, Id, "green"), Id);
        }

        public string GetLinkText()
        {
            return Link;
        }
    }
}
    
