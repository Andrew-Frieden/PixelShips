using System.Collections.Generic;
using Models.Dtos;
using Models.Stats;
using TextEncoding;
using Links.Colors;

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

        public AttackAction(IRoomActor source, IRoomActor target, int damage, string weapon)
        {
            Source = source;
            Target = target;
            Stats = new Dictionary<string, int>();
            Damage = damage;
            Weapon = weapon;
        }

        public override IEnumerable<string> Execute(IRoom room)
        {
            Target.Stats[StatKeys.Hull] -= Damage;
            Target.IsHostile = true;
            
            PlayerTookDamage(new PlayerTookDamageEventArgs(Damage));

            if (Source is CommandShip)
            {
                //TODO - add target link?
                return new List<string>() { ("< > deal " + Damage + " damage to the target with your " + Weapon + ".").Encode(Source.GetLinkText(), Source.Id, LinkColors.Player)};
            }
            else
            {
                return new List<string>() { ("< > dealt you " + Damage + " damage with it's " + Weapon + ".").Encode(Source.GetLinkText(), Source.Id, LinkColors.HostileEntity)};
            }

        }
    }
}