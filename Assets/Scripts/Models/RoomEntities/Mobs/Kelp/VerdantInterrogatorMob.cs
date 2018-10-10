using System.Collections.Generic;
using System.Linq;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
using TextEncoding;
using UnityEngine;

namespace Models.RoomEntities.Mobs.Kelp
{
    public class VerdantInterrogatorMob : Mob
    {
        public VerdantInterrogatorMob()
        {
            Name = "Verdant Interrogator";
            IsHostile = false;
            IsAttackable = true;
            Values[ValueKeys.LookText] = "A <> is in the sector, guns at the ready.";
            Values[ValueKeys.LookTextAggro] = "A <> is maneuvering to attack position.";
            Stats[StatKeys.Hull] = 35;
        }

        public override void CalculateDialogue(IRoom room)
        {
            DialogueContent = DialogueBuilder.PlayerAttackDialogue("Verdant Interrogator\n" +
                                                                   "Hull: " + Stats[StatKeys.Hull] + " / 35\n" +
                                                                   "An overgrown captial class ship that wants to \"ask\" you some \"questions\".\n" +
                                                                   "They will probably destroy you.", this, room);
        }

        public override IRoomAction MainAction(IRoom room)
        {
            if (!IsHostile)
            {
                return new BecomeHostileAction(this, 5);
            }
            
            var actionList = new List<IRoomAction>
            {
                new AttackAction(this, room.PlayerShip, 2, "Blossom Cannon", 0),
                new AttackAction(this, room.PlayerShip, 2, "Blossom Cannon", 0),
                new CreateDelayedAttackActorAction(this, room.PlayerShip, 2, 8, "Leech Torpedo", 0),
                new AttackAction(this, room.PlayerShip, 12, "Great Blossom Cannon", 0)
            };

            return actionList.OrderBy(d => System.Guid.NewGuid()).First();
        }

        public VerdantInterrogatorMob(FlexEntityDto dto) : base(dto) { }

        public VerdantInterrogatorMob(FlexData data) : base(data) { }
    }
}