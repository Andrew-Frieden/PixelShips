using System;
using System.Collections.Generic;
using Models.Dtos;

namespace Models.Actions
{
    public abstract class SimpleAction : IRoomAction
    {
        public string ActionName;

        public IRoomActor Target;
        public IRoomActor Source;
        public Dictionary<string, int> Stats;
        public Dictionary<string, string> Values;

        public abstract IEnumerable<string> Execute(IRoom room);
        
        protected SimpleAction(SimpleActionDto dto, IRoom room)
        {
            Source = (IRoomActor)room.FindEntity(dto.SourceId);
            Target = (IRoomActor)room.FindEntity(dto.TargetId);
            Stats = dto.Stats;
            Values = dto.Values;
        }
        
        protected SimpleAction()
        {
            Stats = new Dictionary<string, int>();
            Values = new Dictionary<string, string>();
        }
        
        public delegate void OnPlayerTookDamageEvent();
        public static event OnPlayerTookDamageEvent onPlayerTookDamageEvent;
        
        protected virtual void PlayerTookDamage()
        {
            onPlayerTookDamageEvent?.Invoke();
        }
    }
}
