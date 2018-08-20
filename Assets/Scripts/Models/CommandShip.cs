﻿using System;

namespace Models
{
    public class CommandShip : Ship, ICombatEntity, ITextEntity
    {
        public string Id { get; }
        public ABDialogueContent DialogueContent { get; }

        public string GetLookText()
        {
            return "[You] warped into the sector.";
        }

        public string GetLinkText()
        {
            throw new System.NotImplementedException();
        }

        public CommandShip(int gathing, int transport, int intelligence, int combat, int speed, int hull) : base(gathing, transport, intelligence, combat, speed, hull)
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}