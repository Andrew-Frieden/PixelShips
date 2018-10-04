using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
using TextEncoding;
using UnityEngine;

namespace Models.RoomEntities.Mobs.Kelp
{
    public class VerdantObserverMob : Mob
    {
        public VerdantObserverMob()
        {
            Name = "Verdant Observer";
            IsHostile = false;
            IsAttackable = true;
            Values[ValueKeys.LookText] = "A <> is in the sector, guns at the ready.";
            Values[ValueKeys.LookTextAggro] = "A <> is maneuvering to attack position.";
            Stats[StatKeys.Hull] = 3;
        }

        public override void CalculateDialogue(IRoom room)
        {
            DialogueContent = DialogueBuilder.PlayerAttackDialogue("Verdant Observer\n" +
                                                                   "Hull: " + Stats[StatKeys.Hull] + " / 3\n" +
                                                                   "A ship so overgrown it's hard to make out what model it originally was.\n" +
                                                                   "You can take them.", this, room);
        }

        public override IRoomAction MainAction(IRoom room)
        {
            if (!IsHostile)
            {
                return new BecomeHostileAction(this, 5);
            }

            return new AttackAction(this, room.PlayerShip, 2, "Blossom Cannon", 0);
        }

        public VerdantObserverMob(FlexEntityDto dto, IRoom room) : base(dto, room) { }

        public VerdantObserverMob(Dictionary<string, int> stats, Dictionary<string, string> values) : base(stats, values) { }
    }
}