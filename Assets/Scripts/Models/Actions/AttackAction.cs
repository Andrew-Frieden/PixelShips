using System.Collections.Generic;
using Models.Dtos;
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

        public AttackAction(IRoomActor source, IRoomActor target, int damage)
        {
            Source = source;
            Target = target;
            Stats = new Dictionary<string, int>();
            Damage = damage;
        }

        public override IEnumerable<string> Execute(IRoom room)
        {
            Target.Stats["Hull"] -= Damage;
            Target.IsAggro = true;

            if (Source is CommandShip)
            {
                //TODO - add target link?
                return new List<string>() { ("< > deal " + Damage + " damage to the target.").Encode(Source.GetLinkText(), Source.Id, "red")};
            }
            else
            {
                return new List<string>() { ("< > dealt you " + Damage + " damage.").Encode(Source.GetLinkText(), Source.Id, "red")};
            }

        }
    }
}