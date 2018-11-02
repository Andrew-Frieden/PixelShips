using System;
using System.Collections.Generic;
using TextSpace.Models.Actions;
using TextSpace.Models.Dialogue;
using TextSpace.Models.Dtos;
using TextEncoding;
using TextSpace.Services.Factories;
using TextSpace.Framework.IoC;
using TextSpace.Services;

namespace TextSpace.Models
{
    public class Room : IRoom
    {
        public string Name;
        public string LookText;
        public string Id { get; private set; }
        public string Description { get; private set; }
        public IEnumerable<RoomTemplate> Exits { get; private set; }
        public RoomFlavor Flavor { get; }
        public List<IRoomActor> Entities { get; private set; }
        public ABDialogueContent DialogueContent { get; set; }

        private CommandShip _cmdShip;
        public CommandShip PlayerShip
        {
            get
            {
                if (_cmdShip == null)
                    _cmdShip = ServiceContainer.Resolve<IExpeditionProvider>().Expedition.CmdShip;
                return _cmdShip;
            }
        }

        public Room(RoomInjectable inject, IEnumerable<RoomTemplate> exits, List<IRoomActor> entities)
        {
            Id = Guid.NewGuid().ToString();
            Name = GetNameForFlavor(inject.Flavor);
            Description = inject.Description;
            LookText = inject.LookText;
            Exits = exits;
            Entities = entities;
        }

        public Room(RoomDto dto)
        {
            Id = dto.Id;
            Description = dto.Description;
            LookText = dto.LookText;
            Name = dto.Name;
            Flavor = dto.Flavor;

            Entities = new List<IRoomActor>();
            dto.Entities.ForEach(n => Entities.Add(n.FromDto()));

            var roomExits = new List<RoomTemplate>();
            dto.ExitDtos.ForEach(x => roomExits.Add(new RoomTemplate(x)));
            Exits = roomExits;
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
            DialogueContent = ServiceContainer.Resolve<NavigationService>().NavigationDialogue();
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
    
