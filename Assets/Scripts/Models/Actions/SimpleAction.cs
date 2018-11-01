using System;
using System.Collections.Generic;
using TextSpace.Models.Dtos;
using TextSpace.Models.Stats;
using UnityEngine;

namespace TextSpace.Models.Actions
{
    public abstract class SimpleAction : IRoomAction
    {
        public bool IsValid { get; set; }

        protected int BaseDamage
        {
            get
            {
                return Stats[StatKeys.BaseDamage];
            }
            set
            {
                Stats[StatKeys.BaseDamage] = value;
            }
        }

        protected int TimeToLive
        {
            get
            {
                return Stats[StatKeys.TimeToLive];
            }
            set
            {
                Stats[StatKeys.TimeToLive] = value;
            }
        }

        protected string Name
        {
            get
            {
                return Values[ValueKeys.Name];
            }
            set
            {
                Values[ValueKeys.Name] = value;
            }
        }

        public int Energy
        {
            get
            {
                if (!Stats.ContainsKey(StatKeys.Energy))
                    Energy = 0;
                return Stats[StatKeys.Energy];
            }
            protected set
            {
                Stats[StatKeys.Energy] = value;
            }
        }

        public int Resourcium
        {
            get
            {
                if (!Stats.ContainsKey(StatKeys.Resourcium))
                    Resourcium = 0;
                return Stats[StatKeys.Resourcium];
            }
            protected set
            {
                Stats[StatKeys.Resourcium] = value;
            }
        }

        public IRoomActor Target;
        public IRoomActor Source;
        public Dictionary<string, int> Stats;
        public Dictionary<string, string> Values;

        private List<UIResponseTag> _actionTags;
        protected List<UIResponseTag> ActionTags
        {
            get
            {
                if (_actionTags != null)
                {
                    return _actionTags;
                }
                
                _actionTags = new List<UIResponseTag>();
                if (Energy > 0 && Source != null && Source is CommandShip)
                {
                    _actionTags.Add(UIResponseTag.PlayerEnergyConsumed);
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

            //  TODO this is wierd. we should use the template pattern so we can always call this logic
            return null;
        }
        
        //public SimpleAction(SimpleActionDto dto, IRoom room)
        //{
        //    if (!string.IsNullOrEmpty(dto.SourceId))
        //        Source = (IRoomActor)room.FindEntity(dto.SourceId);

        //    if (!string.IsNullOrEmpty(dto.TargetId))
        //        Target = (IRoomActor)room.FindEntity(dto.TargetId);

        //    Stats = dto.Stats;
        //    Values = dto.Values;
        //}

        protected SimpleAction()
        {
            Stats = new Dictionary<string, int>();
            Values = new Dictionary<string, string>();
        }

        public virtual void CalculateValid(IRoom room)
        {
            if (Source == null)
            {
                IsValid = true;
                return;
            }

            //TODO: possible that one has energy and other doesnt, this should throw an exception
            if (!Source.Stats.ContainsKey(StatKeys.Energy) || !Stats.ContainsKey(StatKeys.Energy))
            {
                IsValid = true;
            }
            else
            {
                IsValid = Source.Stats[StatKeys.Energy] >= Stats[StatKeys.Energy];
            }
        }
    }
}
