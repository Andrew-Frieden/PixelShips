﻿using System.Collections.Generic;
using Models.Stats;
using UnityEngine;

namespace Models.Actions
{
    public class PassiveRoomHealAction : SimpleAction
    {
        public PassiveRoomHealAction()
        {
            Stats = new Dictionary<string, int>();
        }

        public override IEnumerable<StringTagContainer> Execute(IRoom room)
        {
            //Heal the player shields for 5, max of max shields
            room.PlayerShip.Stats[StatKeys.Shields] = Mathf.Min(room.PlayerShip.Stats[StatKeys.MaxShields],
                room.PlayerShip.Stats[StatKeys.Shields] + 5);
            
            //Heal all entity shields for 3, max of max shields
            foreach (var entity in room.Entities)
            {
                if (entity.Stats.ContainsKey(StatKeys.Shields))
                {
                    entity.Stats[StatKeys.Shields] = Mathf.Min(entity.Stats[StatKeys.MaxShields],
                        entity.Stats[StatKeys.Shields] + 3);
                }
            }
            
            return new List<StringTagContainer>()
            {
                new StringTagContainer()
                {
                    Text = "The march of time continues. Your shields regenerate.",
                    ResultTags = new List<ActionResultTags> { ActionResultTags.Heal }
                }
            };
        }
    }
}