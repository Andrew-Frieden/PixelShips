using Models;
using Models.Actions;
using Models.Dtos;
using Models.Stats;

namespace Items
{
    public class CreateDelayedAttackActorWeapon : Weapon
    {
        public int TimeToLive
        {
            get
            {
                return Stats[StatKeys.TimeToLive];
            }
            private set
            {
                Stats[StatKeys.TimeToLive] = value;
            }
        }
        
        public CreateDelayedAttackActorWeapon(FlexEntityDto dto) : base(dto) { }

        public CreateDelayedAttackActorWeapon(FlexData data) : base(data) { } 
        
        public override IRoomAction MainAction(IRoom room)
        {
            return new DoNothingAction(this);
        }

        public override IRoomAction GetAttackAction(IRoom room, IRoomActor src, IRoomActor target)
        {
            return new CreateDelayedAttackActorAction(src, target, TimeToLive, BaseDamage, Name, Energy);
        }
    }
}