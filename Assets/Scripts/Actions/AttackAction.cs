using System.Collections.Generic;
using System.Linq;
using Models;

namespace Actions
{
    public class AttackAction : IRoomAction
    {
        private int Damage { get; }
        private ICombatEntity source;
        private ICombatEntity target;
        
        public AttackAction(ICombatEntity source, ICombatEntity target)
        {
            Damage = 5;
        }

        public IEnumerable<string> Execute(IRoom room)
        {
            target.Hull -= Damage;

            var targetEntity = room.Entities.Single(entity => entity.Id == target.Id);

            return new List<string>() {targetEntity.GetLinkText() + " took " + Damage + " damage."};
        }
    }
}