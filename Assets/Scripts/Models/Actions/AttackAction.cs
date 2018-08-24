using System.Collections.Generic;
using Helpers;
using Models.Dtos;

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

        public AttackAction(IRoom room, SimpleActionDto dto)
        {
            ActionName = "Attack";
            Source = (ICombatEntity)room.FindEntity(dto.SourceId);
            Target = (ICombatEntity)room.FindEntity(dto.TargetId);
            Stats = dto.Stats;
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
            return new List<string>() { room.FindTextEntityByGuid(Target.Id).GetLinkText() + " took " + Damage + " damage."};
        }
    }
}