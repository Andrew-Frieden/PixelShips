using Models;
using Models.Actions;
using Models.Dtos;

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

        public override IRoomAction GetAttackAction(IRoom room, IRoomActor src, IRoomActor target)
        {
            return new AttackAction(src, target, BaseDamage, Name, Energy);
        }
    }
}