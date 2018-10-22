using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Factories;
using TextEncoding;

namespace Models
{
    public class Room : IRoom
    {
        public string Name;
        public string LookText;
        private string description;

        public string Id { get; private set; }
        public string Description { get { return description; } }

        public IEnumerable<RoomTemplate> Exits { get; private set; }
        public CommandShip PlayerShip { get; private set; }
        public RoomFlavor Flavor { get; }
        public List<IRoomActor> Entities { get; private set; }
        public ABDialogueContent DialogueContent { get; set; }

        public Room(RoomInjectable inject, IEnumerable<RoomTemplate> exits, List<IRoomActor> entities)
        {
            Id = Guid.NewGuid().ToString();
            Name = GetNameForFlavor(inject.Flavor);
            description = inject.Description;
            LookText = inject.LookText;
            Exits = exits;
            Entities = entities;
        }

        public Room(RoomDto dto)
        {
            Id = dto.Id;
            description = dto.Description;
            LookText = dto.LookText;
            Name = dto.Name;
            Flavor = dto.Flavor;

            Entities = new List<IRoomActor>();
            dto.Entities.ForEach(n => Entities.Add(n.FromDto()));

            var roomExits = new List<RoomTemplate>();
            dto.ExitDtos.ForEach(x => roomExits.Add(new RoomTemplate(x)));
            Exits = roomExits;
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

        public void CalculateDialogue()
        {
            DialogueContent = DialogueBuilder.PlayerNavigateDialogue(this);
        }

        public static string GetNameForFlavor(RoomFlavor flavor)
        {
            switch (flavor)
            {
                case RoomFlavor.Empty:
                    return "Empty Space";
                case RoomFlavor.Nebula:
                    return "Nebula Tropics";
                case RoomFlavor.Kelp:
                    return "Kelp Forest";
                case RoomFlavor.Grid:
                    return "Grid Zone";
                case RoomFlavor.Rural:
                    return "Rural Expanse";
                case RoomFlavor.Solar:
                    return "Solar Proxima";
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
    
