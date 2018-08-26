﻿using System;
using System.Collections.Generic;
using PixelShips.Helpers;
using UnityEngine;

namespace Models
{
    public class CommandShip : Ship, ICombatEntity
    {
        public string Id { get; }
        public ABDialogueContent DialogueContent { get; }
        
        private Dictionary<string, IRoomActor> Actors { get; }

        public string GetLookText()
        {
            return "You".GetLink("orange", Id);;
        }

        public string GetLinkText()
        {
            return "You".GetLink("orange", Id);
        }

        public CommandShip(int gathing, int transport, int intelligence, int combat, int speed, int hull) : base(gathing, transport, intelligence, combat, speed, hull)
        {
            Id = Guid.NewGuid().ToString();
            DialogueContent = new ABDialogueContent();
        }

        public void AddActor(IRoomActor actor)
        {
            if (Actors.ContainsKey(actor.Id))
            {
                Actors[actor.Id] = actor;
            }
            else
            {
                Debug.Log("Error: Ship already contains that actor.");
            }
        }

        public void RemoveActor(string id)
        {
            if (Actors.ContainsKey(id))
            {
                Actors[id] = null;
            }
            else
            {
                Debug.Log("Error: Ship does not contain that actor.");
            }
        }

        public CommandShip() { }
    }
}