using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dtos;
using Models.Stats;

namespace Models
{
    public abstract class FlexEntity : IRoomActor
    {
        public bool IsHidden { get; protected set; }

        public bool IsHostile
        {
            get
            {
                if (!Stats.ContainsKey(StatKeys.IsHostile))
                    Stats[StatKeys.IsHostile] = 0;
                return Stats[StatKeys.IsHostile] != 0;
            }
            set
            {
                Stats[StatKeys.IsHostile] = value ? 1 : 0;
            }
        }

        public bool IsAttackable
        {
            get
            {
                return Stats[StatKeys.CanCombat] != 0;
            }
            set
            {
                Stats[StatKeys.CanCombat] = value ? 1 : 0;
            }
        }
        
        public string Id { get; private set; }

        public string Name
        {
            get
            {
                return Values[ValueKeys.Name];
            }
            protected set
            {
                Values[ValueKeys.Name] = value;
            }
        }
        
        public Dictionary<string, int> Stats { get; protected set; }
        public Dictionary<string, string> Values { get; protected set; }
        public ABDialogueContent DialogueContent { get; set; }

        const string CurrentStateKey = "current_state"; 
        protected int CurrentState 
        { 
            get
            {
                if (!Stats.ContainsKey(CurrentStateKey))
                {
                    Stats[CurrentStateKey] = 0;
                }
                return Stats[CurrentStateKey]; 
            }
        }

        public virtual bool IsDestroyed => false;

        public void ChangeState(int nextState)
        {
            Stats[CurrentStateKey] = nextState;
        }

        protected FlexEntity(FlexEntityDto dto, IRoom room)
        {
            Values = dto.Values;
            Stats = dto.Stats;
            Id = dto.Id;
        }

        protected FlexEntity()
        {
            Id = Guid.NewGuid().ToString();
            Stats = new Dictionary<string, int>();
            Values = new Dictionary<string, string>();
        }

        protected FlexEntity(Dictionary<string, int> stats, Dictionary<string, string> values)
        {
            Id = Guid.NewGuid().ToString();
            Stats = stats;
            Values = values;
        }

        public string GetLinkText()
        {
            return Name;
        }

        public abstract IRoomAction GetNextAction(IRoom room);
        public abstract ABDialogueContent CalculateDialogue(IRoom room);
        public abstract string GetLookText();

        public virtual void AfterAction(IRoom room) 
        {
            //  do nothing, but allow child classes to override this
        }
    }

    public class FlexData
    {
        public Dictionary<string, int> Stats;
        public Dictionary<string, string> Values;
    }
}
