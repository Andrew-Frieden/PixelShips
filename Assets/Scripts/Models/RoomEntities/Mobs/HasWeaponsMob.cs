using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;

namespace Models.RoomEntities.Mobs
{
    public class HasWeaponsMob: Mob
    {
        public HasWeaponsMob()
        {
            IsHostile = false;
            IsAttackable = true;
        }

        public override void CalculateDialogue(IRoom room)
        {
            DialogueContent = DialogueBuilder.PlayerAttackDialogue(Values[ValueKeys.Name] + "\n" +
                                                                   "Hull: " + Stats[StatKeys.Hull] + " / " + Stats[StatKeys.MaxHull] + "\n",
                                                                    this, room);
        }

        public override IRoomAction MainAction(IRoom room)
        {
            if (!IsHostile)
            {
                return new BecomeHostileAction(this, 5);
            }

            return new CreateDelayedAttackActorAction(this, room.PlayerShip, 2, 8, "Leech Torpedo", 0);
        }

        public HasWeaponsMob(FlexEntityDto dto) : base(dto) { }

        public HasWeaponsMob(FlexData data) : base(data) { }
    }
}