using System;
using System.Collections.Generic;
using System.Linq;
using Models.Actions;
using Models.Factories;
using TextEncoding;
using UnityEngine;

namespace Models
{
    public class Room : IRoom
    {
        private string Link { get; }
        
        public string Id { get; }
        public int _tick { get; private set; }
        public string Description { get; set; }

        public IRoom Exit { get; set; }
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
            RoomTemplates = templates;
        }

        public Room(string id, string desc, string name)
        {
            Id = id;
            Description = desc;
            Link = name;
            Entities = new List<IRoomActor>();
        }
        
        //TODO: params out of order here
        public Room(string name, string desc)
        {
            Id = Guid.NewGuid().ToString();
            Description = desc;
            Link = name;
            Entities = new List<IRoomActor>();
        }

        public Room()
        {
            Entities = new List<IRoomActor>();
        }
        
        public void Tick()
        {
            _tick++;
        }

        public List<RoomTemplate> RoomTemplates { get; set; }

        public List<string> ResolveNext(IRoomAction playerAction)
        {
            var actions = new List<IRoomAction>() {playerAction};
            Entities.ForEach(e => actions.Add(e.GetNextAction(this)));

            var resultText = new List<string>();
            actions.ForEach(a => resultText.AddRange(a.Execute(this)));

            return resultText;
        }

        public void SetPlayerShip(CommandShip ship)
        {
            PlayerShip = ship;
        }

        public void AddEntity(IRoomActor actor)
        {
            Entities.Add(actor);
        }

        public string GetLookText()
        {
            return Description.Encode(Link, Id, "green");
        }

        public string GetLinkText()
        {
            return Link;
        }
    }
}
    
