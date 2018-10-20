using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Models;
using Models.Actions;
using Models.Dtos;
using Models.Stats;
using TextEncoding;

namespace Items
{
    public abstract class Weapon : FlexEntity
    {
        public enum WeaponState
        {
            Unequipped = 0,
            Equipped = 1
        }
        
        public enum WeaponTypes
        {
            Light = 0,
            Heavy = 1
        }
        
        public WeaponTypes WeaponType
        {
            get
            {
                return (WeaponTypes) Stats[StatKeys.WeaponType];
            }
            protected set
            {
                Stats[StatKeys.WeaponType] = (int) value;
            }
        }
        
        public int BaseDamage
        {
            get
            {
                return Stats[StatKeys.BaseDamage];
            }
            protected set
            {
                Stats[StatKeys.BaseDamage] = value;
            }
        }
        
        public int Energy
        {
            get
            {
                return Stats[StatKeys.Energy];
            }
            protected set
            {
                Stats[StatKeys.Energy] = value;
            }
        }
        
        public Weapon(FlexEntityDto dto) : base(dto) { }

        public Weapon(FlexData data) : base(data) { }

        public abstract IRoomAction GetAttackAction(IRoom room, IRoomActor src, IRoomActor target);

        public void SetDependentActorId(string id)
        {
            DependentActorId = id;
        }
        
        public override TagString GetLookText()
        {
            return LookText.Encode(this, LinkColors.Weapon).Tag();
        }
        
        public class PickupWeaponAction : SimpleAction
        {
            public PickupWeaponAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }

            public PickupWeaponAction(IRoomActor src, IRoomActor target)
            {
                if (target is Weapon)
                    Target = target;
                else
                    throw new InvalidOperationException();

                Source = src;
            }

            public override IEnumerable<TagString> Execute(IRoom room)
            {
                var weapon = (Weapon)Target;

                if (Source == room.PlayerShip)
                {
                    var previousWeapon = room.PlayerShip.SwapWeapon(weapon);
                    weapon.IsDestroyed = true;
                    weapon.ChangeState((int) WeaponState.Equipped);

                    room.AddEntity(previousWeapon);
                    previousWeapon.ChangeState((int) WeaponState.Unequipped);
                    
                    return new List<TagString>()
                    {
                        $"You pickup the <>".Encode(weapon, LinkColors.Weapon).Tag(),
                        $"You drop your <>".Encode(previousWeapon, LinkColors.Weapon).Tag()
                    };
                }
                
                //This means a non-Playship is trying to equip a weapon. This is not implemented.
                throw new NotImplementedException();
            }
        }
    }
}