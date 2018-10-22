using System;
using System.Collections.Generic;
using Helpers;
using Models.Dtos;
using Models.Stats;
using TextEncoding;

namespace Models.Actions
{
    public class AttackAction : SimpleAction
    {
        private const string WeaponKey = "weapon";
        private string Weapon
        {
            get
            {
                return Values[WeaponKey];
            }
            set
            {
                Values[WeaponKey] = value;
            }
        }

        public AttackAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }

        public AttackAction(IRoomActor source, IRoomActor target, int damage, string weapon, int energy)
        {
            Source = source;
            Target = target;
            Stats = new Dictionary<string, int>();
            BaseDamage = damage;
            Weapon = weapon;
            Energy = energy;
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            base.Execute(room);
            
            var actualDamage = Target.TakeDamage(BaseDamage);
            Target.IsHostile = true;
            
            if (Source is CommandShip)
            {
                return new List<TagString>()
                {
                    new TagString()
                    {
                        Text = ("< > deal " + actualDamage + " damage to the target with your " + Weapon + ".").Encode(Source.GetLinkText(), Source.Id, LinkColors.Player),
                        Tags = ActionTags
                    }
                };
            }
            else
            {
                ActionTags.Add(UIResponseTag.PlayerDamaged);
                return new List<TagString>()
                {

                    new TagString()
                    {
                        Text = ("< > dealt you " + actualDamage + " damage with it's " + Weapon + ".").Encode(Source.GetLinkText(), Source.Id, LinkColors.HostileEntity),
                        Tags = ActionTags
                    }
                };
            }
        }
    }
}