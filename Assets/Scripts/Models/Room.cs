using System;
using System.Collections.Generic;
using Models.Actions;
using TextEncoding;

namespace Models
{
    public class Room : IRoom
    {
        private string Link { get; }
        
        public string Id { get; }
        public string Description { get; set; }

        public List<RoomTemplate> Exits { get; set; }
        public CommandShip PlayerShip { get; private set; }
        public RoomFlavor Flavor { get; }
        public List<IRoomActor> Entities { get; }
        public ABDialogueContent DialogueContent { get; set; }

        public Room(string description, string link, CommandShip ship, List<IRoomActor> roomEntities, ABDialogueContent dialogueContent, List<RoomTemplate> templates)
        {
            Id = Guid.NewGuid().ToString();
            Description = description;
            Link = link;
            PlayerShip = ship;
            Entities = roomEntities;
            DialogueContent = dialogueContent;
            Exits = templates;
        }

        public Room(string id, string desc, string name)
        {
            Id = id;
            Description = desc;
            Link = name;
            Entities = new List<IRoomActor>();
        }
        
        public Room(string name, string description)
        {
            Id = Guid.NewGuid().ToString();
            Description = description;
            Link = name;
            Entities = new List<IRoomActor>();
        }

        public Room()
        {
            Entities = new List<IRoomActor>();
        }

        public void SetPlayerShip(CommandShip ship)
        {
            PlayerShip = ship;
        }

        public void AddEntity(IRoomActor actor)
        {
            Entities.Add(actor);
        }

        public TagString GetLookText()
        {
            return new TagString()
            {
                Text = Description.Encode(Link, Id, LinkColors.Room)
            };
        }

        public string GetLinkText()
        {
            return Link;
        }
    }
}
    
