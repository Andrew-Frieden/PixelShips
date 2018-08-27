using System;
using System.Collections.Generic;
using System.Linq;
using Models.Actions;
using PixelShips.Helpers;
using UnityEngine;

namespace Models
{
    public class Room : IRoom
    {
        public string Link { get; }
        
        public string Id { get; }
        public CommandShip PlayerShip { get; private set; }
        public string Description { get; }
        public RoomFlavor Flavor { get; }
        public List<IRoomActor> Entities { get; }
        public ABDialogueContent DialogueContent { get; set; }

        public Room(string description, string link, CommandShip ship, List<IRoomActor> roomEntities, ABDialogueContent dialogueContent)
        {
            Id = Guid.NewGuid().ToString();
            Description = description;
            Link = link;
            PlayerShip = ship;
            Entities = roomEntities;
            DialogueContent = dialogueContent;
        }

        public Room(string id, string desc, string name)
        {
            Id = id;
            Description = desc;
            Link = name;
            Entities = new List<IRoomActor>();
        }

        public Room()
        {
            Entities = new List<IRoomActor>();
        }

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
            return Description.GetDescriptionWithLink(Link, Id, "green");
        }

        public string GetLinkText()
        {
            return Link;
        }

        public IRoomActor FindRoomActorByGuid(string id)
        {
            var actor = Entities.FirstOrDefault(entity => entity.Id == id);
            if (actor == null)
            {
                Debug.Log($"Error: No actor found by the id: {id}");
            }
            return actor;
        }
    }
}
    
