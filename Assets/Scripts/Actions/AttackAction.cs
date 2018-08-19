using System.Collections.Generic;
using System.Linq;
using Helpers;
using Models;

namespace Actions
{
    public class AttackAction : IRoomAction
    {
        private int Damage { get; }
        private ICombatEntity Source;
        private ICombatEntity Target;
        
        public AttackAction(ICombatEntity source, ICombatEntity target, int damage)
        {
            Source = source;
            Target = target;
            Damage = damage;
        }

        public IEnumerable<string> Execute(IRoom room)
        {
            Target.Hull -= Damage;

            return new List<string>() {room.FindTextEntityByGuid(Target.Id).GetLinkText() + " took " + Damage + " damage."};
        }
    }
}