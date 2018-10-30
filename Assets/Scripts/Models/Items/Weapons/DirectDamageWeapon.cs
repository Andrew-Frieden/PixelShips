using TextSpace.Models;
using TextSpace.Models.Actions;
using TextSpace.Models.Dtos;

namespace TextSpace.Items
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