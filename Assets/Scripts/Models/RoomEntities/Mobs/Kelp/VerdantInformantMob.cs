using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
using TextEncoding;
using UnityEngine;

namespace Models.RoomEntities.Mobs
{
    public class VerdantInformantMob : Mob
    {
        public VerdantInformantMob()
        {
            Name = "Verdant Informant";
            IsHostile = false;
            IsAttackable = true;
        }

        public override void CalculateDialogue(IRoom room)
        {
            DialogueContent = DialogueBuilder.PlayerAttackDialogue(Values[ValueKeys.Name] + "\n" +
                                                                   "Hull: " + Stats[StatKeys.Hull] + " / " + Stats[StatKeys.MaxHull] + "\n" +
                                                                   "This ship is definitely up to something, but it's very difficult to tell what.\n" +
                                                                   "You can probably take them.", this, room);
        }

        public override IRoomAction MainAction(IRoom room)
        {
            if (!IsHostile)
            {
                return new BecomeHostileAction(this, 5);
            }

            return new CreateDelayedAttackActorAction(this, room.PlayerShip, 2, 8, "Leech Torpedo", 0);
        }

        public VerdantInformantMob(FlexEntityDto dto) : base(dto) { }

        public VerdantInformantMob(FlexData data) : base(data) { }
    }
}