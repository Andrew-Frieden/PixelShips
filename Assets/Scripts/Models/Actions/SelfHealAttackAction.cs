using System;
using System.Collections.Generic;
using Helpers;
using TextSpace.Models.Dtos;
using TextSpace.Models.Stats;
using TextEncoding;

namespace TextSpace.Models.Actions
{
    public class SelfHealAttackAction : SimpleAction
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

        private int BaseHeal;

        public SelfHealAttackAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }

        public SelfHealAttackAction(IRoomActor source, IRoomActor target, int damage, int heal, string weapon, int energy)
        {
            Source = source;
            Target = target;
            Stats = new Dictionary<string, int>();
            BaseDamage = damage;
            BaseHeal = heal;
            Weapon = weapon;
            Energy = energy;
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            base.Execute(room);
            
            var actualDamage = Target.TakeDamage(BaseDamage);
            var actualHeal = Source.HealDamage(BaseHeal);

            Target.IsHostile = true;
            
            if (Source is CommandShip)
            {
                ActionTags.Add(UIResponseTag.PlayerHealed);
                return new List<TagString>()
                {
                    new TagString()
                    {
                        Text = ("< > deal " + actualDamage + " damage and heal " + actualHeal + " with your " + Weapon + ".").Encode(Source.GetLinkText(), Source.Id, LinkColors.Player),
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
                        Text = ("< > dealt you " + actualDamage + " damage and healed " + actualHeal +  " with it's " + Weapon + ".").Encode(Source.GetLinkText(), Source.Id, LinkColors.HostileEntity),
                        Tags = ActionTags
                    }
                };
            }
        }
    }
}