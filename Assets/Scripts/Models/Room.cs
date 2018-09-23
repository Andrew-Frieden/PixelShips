using System;
using System.Collections.Generic;
using Models.Actions;
using TextEncoding;

namespace Models
{
    public class Room : IRoom
    {
        private string Name;
        private string LookText;
        private string description;

        public string Id { get; }
        public string Description { get { return description; } }

        public IEnumerable<RoomTemplate> Exits { get; private set; }
        public CommandShip PlayerShip { get; private set; }
        public RoomFlavor Flavor { get; }
        public List<IRoomActor> Entities { get; private set; }
        public ABDialogueContent DialogueContent { get; set; }

        public Room(RoomInjectable inject, IEnumerable<RoomTemplate> exits, List<IRoomActor> entities) : this()
        {
            Name = GetNameForFlavor(inject.Flavor);
            description = inject.Description;
            LookText = inject.LookText;
            Exits = exits;
            Entities = entities;
        }

        private Room()
        {
            Id = Guid.NewGuid().ToString();
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
                Text = LookText.Encode(Name, Id, LinkColors.Room)
            };
        }

        public string GetLinkText()
        {
            return Name;
        }

        public static string GetNameForFlavor(RoomFlavor flavor)
        {
            switch (flavor)
            {
                case RoomFlavor.Empty:
                    return "Empty Space";
                case RoomFlavor.Nebula:
                    return "Nebula";
                case RoomFlavor.Kelp:
                    return "Kelp Forest";
                default:
                    return "Null Space";
            }
        }
    }

    public class RoomInjectable
    {
        public string LookText;
        public string Description;
        public RoomFlavor Flavor;

        public RoomInjectable(RoomFlavor flavor, string look, string desc)
        {
            LookText = look;
            Description = desc;
            Flavor = flavor;
        }
    }
}
    
