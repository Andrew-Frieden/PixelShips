using System;
using System.Collections.Generic;
using PixelShips.Helpers;
using UnityEngine;

namespace Models
{
    public class CommandShip : Ship, ICombatEntity
    {
        public string Id { get; }
        public ABDialogueContent DialogueContent { get; set; }

        public string GetLookText()
        {
            throw new NotImplementedException();
        }

        public string GetLinkText()
        {
            return "You";
        }

        public CommandShip(int gathering, int transport, int intelligence, int combat, int speed, int hull) : base(gathering, transport, intelligence, combat, speed, hull)
        {
            Id = Guid.NewGuid().ToString();
            DialogueContent = new ABDialogueContent();
        }

        public CommandShip(string id, int gathering, int transport, int intelligence, int combat, int speed, int hull) : base(gathering, transport, intelligence, combat, speed, hull)
        {
            Id = id;
            DialogueContent = null;
        }
    }
}