using Models;
using Models.Actions;
using Models.Dtos;
using Models.Stats;

namespace Items
{
    public class SelfHealWeapon: Weapon
    {
        private int BaseHeal
        {
            get
            {
                return Stats[StatKeys.BaseHeal];
            }
            set
            {
                Stats[StatKeys.BaseHeal] = value;
            }
        }
        
        public SelfHealWeapon(FlexEntityDto dto) : base(dto) { }

        public SelfHealWeapon(FlexData data) : base(data) { }
        
        public override IRoomAction MainAction(IRoom room)
        {
            return new DoNothingAction(this);
        }

        public override IRoomAction GetAttackAction(IRoom room, IRoomActor src, IRoomActor target)
        {
            return new SelfHealAttackAction(src, target, BaseDamage, BaseHeal, Name, Energy);
        }
    }
}