using System;
using System.Linq;
using TextSpace.Items;
using TextSpace.Models.Actions;
using TextSpace.Models.Dialogue;
using TextSpace.Models.Dtos;
using TextSpace.Models.Stats;

namespace TextSpace.Models.RoomEntities.Mobs
{
    public class HasWeaponsMob: Mob
    {
        public HasWeaponsMob()
        {
            IsHostile = true;
            IsAttackable = true;
        }

        public override void CalculateDialogue(IRoom room)
        {
            DialogueContent = DialogueBuilder.PlayerAttackDialogue(Values[ValueKeys.Name] + "\n" +
                                                                   "Hull: " + Stats[StatKeys.Hull] + " / " + Stats[StatKeys.MaxHull] + "\n" +
                                                                   "Shield: " + Stats[StatKeys.Shields] + "/" + Stats[StatKeys.MaxShields] + "\n",
                                                                    this, room);
        }
        
        //TODO: Get weighting working and check IsValid
        public override IRoomAction MainAction(IRoom room)
        {
            var dependentActors = room.FindDependentActors(Id);

            foreach (var actor in dependentActors)
            {
                if (actor is Weapon)
                {
                        
                    return ((Weapon) actor).GetAttackAction(room, this, room.PlayerShip);
                }
            }

            throw new NotImplementedException();
        }

        public HasWeaponsMob(FlexEntityDto dto) : base(dto) { }

        public HasWeaponsMob(FlexData data) : base(data) { }
    }
}