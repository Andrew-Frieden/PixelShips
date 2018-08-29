using System;
using Models.Actions;

namespace Models.Actors
{
    public class ShieldActor : DelayedActor
    {
        private readonly ICombatEntity _source;
        private readonly ICombatEntity _target;
        private readonly int _damageReduction;

        public ShieldActor(ICombatEntity source, ICombatEntity target, int timeToLive, int damageReduction) :base()
        {
            Id = Guid.NewGuid().ToString();
            Stats[TimeToLiveKey] = timeToLive;
            _source = source;
            _target = target;
            _damageReduction = damageReduction; 
        }

        public override string GetLookText()
        {
            return "";
        }

        public override string GetLinkText()
        {
            return "";
        }

        public override IRoomAction GetNextAction(IRoom s)
        {
            return new DelayedAction($"A shield effects {_source.GetLinkText()} and will end ", Id);
        }
    }
}