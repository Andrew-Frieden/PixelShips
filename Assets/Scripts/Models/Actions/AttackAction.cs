using System;
using System.Collections.Generic;
using Helpers;
using Models.Stats;
using TextEncoding;

namespace Models.Actions
{
    public class AttackAction : SimpleAction
    {
        private const string DamageKey = "damage";

        private int Damage
        {
            get
            {
                return Stats[DamageKey];
            }
            set
            {
                Stats[DamageKey] = value;
            }
        }

        private string Weapon;

        public AttackAction(IRoomActor source, IRoomActor target, int damage, string weapon, int energy)
        {
            Source = source;
            Target = target;
            Stats = new Dictionary<string, int>();
            Damage = damage;
            Weapon = weapon;
            Energy = energy;
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            base.Execute(room);
            
            var actualDamage = Target.TakeDamage(Damage);
            Target.IsHostile = true;
            
            if (Source is CommandShip)
            {
                ActionTags.Add(EventTag.Damage);
                    
                //TODO - add target link?
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
                return new List<TagString>()
                {
                    new TagString()
                    {
                        Text = ("< > dealt you " + actualDamage + " damage with it's " + Weapon + ".").Encode(Source.GetLinkText(), Source.Id, LinkColors.HostileEntity),
                        Tags = new List<EventTag> { EventTag.Damage }
                    }
                };
            }
        }
    }
}