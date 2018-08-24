using System.Collections.Generic;
using Helpers;
using Models;
using PixelShips.Helpers;

namespace Actions
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

        public new ICombatEntity Source
        {
            get
            {
                return (ICombatEntity)base.Source;
            }
            set
            {
                base.Source = value;
            }
        }

        public new ICombatEntity Target
        {
            get
            {
                return (ICombatEntity)base.Target;
            }
            set
            {
                base.Target = value;
            }
        }

        public AttackAction(SimpleAction action)
        {
            ActionName = "Attack";
            Stats = action.Stats;
            Source = (ICombatEntity)action.Source;
            Target = (ICombatEntity)action.Target;
        }

        public AttackAction(ICombatEntity source, ICombatEntity target, int damage)
        {
            ActionName = "Attack";
            Source = source;
            Target = target;
            Stats = new Dictionary<string, int>();
            Damage = damage;
        }

        public override IEnumerable<string> Execute(IRoom room)
        {
            Target.Hull -= Damage;

            if (Source is CommandShip)
            {
                //TODO - add target link?
                return new List<string>() { ("< > deal " + Damage + " damage to the target.").GetDescriptionWithLink(Source.GetLinkText(), Target.Id, "orange")};
            }
            else
            {
                return new List<string>() { ("< > dealt you " + Damage + " damage.").GetDescriptionWithLink(Source.GetLinkText(), Target.Id, "orange")};
            }
        }
    }
}