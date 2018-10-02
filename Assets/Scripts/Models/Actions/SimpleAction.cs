using System;
using System.Collections.Generic;
using Models.Dtos;
using Models.Stats;

namespace Models.Actions
{
    public abstract class SimpleAction : IRoomAction
    {
        public string ActionName;

        public IRoomActor Target;
        public IRoomActor Source;
        public Dictionary<string, int> Stats;
        public Dictionary<string, string> Values;
        
        public int Energy
        {
            get
            {
                return Stats[StatKeys.Energy];
            }
            protected set
            {
                Stats[StatKeys.Energy] = value;
            }
        }

        private List<EventTag> _actionTags;
        protected List<EventTag> ActionTags
        {
            get
            {
                if (_actionTags != null)
                {
                    return _actionTags;
                }
                
                _actionTags = new List<EventTag>();
                if (Energy > 0)
                {
                    _actionTags.Add(EventTag.Energy);
                }

                return _actionTags;
            }
        }

        public virtual IEnumerable<TagString> Execute(IRoom room)
        {
            if (Energy > 0 && Source.Stats[StatKeys.Energy] >= Energy)
            {
                Source.Stats[StatKeys.Energy] -= Energy;
            }

            return null;
        }
        
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

        public virtual bool IsValid()
        {
            if (Source == null)
            {
                return true;
            }
            
            //TODO: possible that one has energy and other doesnt, this should throw an exception
            if (!Source.Stats.ContainsKey(StatKeys.Energy) || !Stats.ContainsKey(StatKeys.Energy))
            {
                return true;
            }
            
            return Source.Stats[StatKeys.Energy] >= Stats[StatKeys.Energy];
        }
    }
}
