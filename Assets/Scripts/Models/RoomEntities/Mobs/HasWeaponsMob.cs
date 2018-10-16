using System;
using System.Linq;
using Items;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
using UnityEngine;

namespace Models.RoomEntities.Mobs
{
    public class HasWeaponsMob: Mob
    {
        private IRoomAction AttackAction;
        
        public HasWeaponsMob()
        {
            IsHostile = true;
            IsAttackable = true;
        }

        public override void CalculateDialogue(IRoom room)
        {
            DialogueContent = DialogueBuilder.PlayerAttackDialogue(Values[ValueKeys.Name] + "\n" +
                                                                   "Hull: " + Stats[StatKeys.Hull] + " / " + Stats[StatKeys.MaxHull] + "\n",
                                                                    this, room);
        }
        
        //TODO: Get weighting working and check IsValid
        public override IRoomAction MainAction(IRoom room)
        {
            if (AttackAction == null)
            {
                var dependentActors = room.FindDependentActors(Id);

                foreach (var actor in dependentActors)
                {
                    if (actor is Weapon)
                    {
                        
                        AttackAction = ((Weapon) actor).GetAttackAction(room, this, room.PlayerShip);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            
            return AttackAction;
        }

        public HasWeaponsMob(FlexEntityDto dto) : base(dto) { }

        public HasWeaponsMob(FlexData data) : base(data) { }
    }
}