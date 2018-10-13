using System.Collections.Generic;
using System.Linq;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
using TextEncoding;
using UnityEngine;

namespace Models.RoomEntities.Mobs
{
    public class VerdantInterrogatorMob : Mob
    {
        public VerdantInterrogatorMob()
        {
            IsHostile = false;
            IsAttackable = true;
        }

        public override void CalculateDialogue(IRoom room)
        {
            DialogueContent = DialogueBuilder.PlayerAttackDialogue(Values[ValueKeys.Name] + "\n" +
                                                                   "Hull: " + Stats[StatKeys.Hull] + " / " + Stats[StatKeys.MaxHull] + "\n" +
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