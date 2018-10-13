using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
using TextEncoding;

namespace Items
{
    public class DirectDamageWeapon : Weapon
    {
        public DirectDamageWeapon(FlexEntityDto dto) : base(dto) { }

        public DirectDamageWeapon(FlexData data) : base(data) { }
        
        public override IRoomAction MainAction(IRoom room)
        {
            return new DoNothingAction(this);
        }

        public override IRoomAction GetAttackAction(IRoomActor src, IRoomActor target)
        {
            return new AttackAction(src, target, BaseDamage, Name, Energy);
        }

        public override void CalculateDialogue(IRoom room)
        {
            switch (CurrentState)
            {
                case (int) WeaponState.Unequipped:
                    DialogueContent = DialogueBuilder.Init()
                        .AddMainText(Values[ValueKeys.DialogueUnequippedText].Encode(this, LinkColors.Weapon))
                        .AddOption("Pickup", new PickupWeaponAction(room.PlayerShip, this))
                        .Build();
                    break;
                case (int) WeaponState.Equipped:
                    DialogueContent = DialogueBuilder.Init()
                        .AddMainText(Values[ValueKeys.DialogueEquippedText].Encode(this, LinkColors.Weapon))
                        .Build();
                    break;
            }
        }
    }
}