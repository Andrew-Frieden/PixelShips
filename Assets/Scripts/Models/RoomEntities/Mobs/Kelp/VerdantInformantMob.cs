using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
using TextEncoding;
using UnityEngine;

namespace Models.RoomEntities.Mobs.Kelp
{
    public class VerdantInformantMob : Mob
    {
        public VerdantInformantMob()
        {
            Name = "Verdant Informant";
            IsHostile = false;
            IsAttackable = true;
            Values[ValueKeys.LookText] = "A <> is in the sector, guns at the ready.";
            Values[ValueKeys.LookTextAggro] = "A <> is maneuvering to attack position.";
            Stats[StatKeys.Hull] = 8;
        }

        public override void CalculateDialogue(IRoom room)
        {
            DialogueContent = DialogueBuilder.PlayerAttackDialogue("Verdant Informant\n" +
                                                                   "Hull: " + Stats[StatKeys.Hull] + " / 8\n" +
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

        public VerdantInformantMob(FlexEntityDto dto, IRoom room) : base(dto, room) { }

        public VerdantInformantMob(Dictionary<string, int> stats, Dictionary<string, string> values) : base(stats, values) { }
    }
}