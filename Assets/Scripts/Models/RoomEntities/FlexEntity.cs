using System;
using System.Collections.Generic;
using TextSpace.Items;
using TextSpace.Models.Actions;
using TextSpace.Models.Dtos;
using TextSpace.Models.Stats;

namespace TextSpace.Models
{
    public abstract class FlexEntity : IRoomActor
    {
        public bool IsHidden
        {
            get
            {
                if (!Stats.ContainsKey(StatKeys.IsHidden))
                    IsHidden = false;
                return Stats[StatKeys.IsHidden] != 0;
            }
            set
            {
                Stats[StatKeys.IsHidden] = value ? 1 : 0;
            }
        }

        public bool IsHostile
        {
            get
            {
                if (!Stats.ContainsKey(StatKeys.IsHostile))
                    IsHostile = false;
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
                return Stats[StatKeys.IsAttackable] != 0;
            }
            set
            {
                Stats[StatKeys.IsAttackable] = value ? 1 : 0;
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

        protected string LookText
        {
            get
            {
                return Values[ValueKeys.LookText];
            }
            set
            {
                Values[ValueKeys.LookText] = value;
            }
        }

        protected string DialogueText
        {
            get
            {
                return Values[ValueKeys.DialogueText];
            }
            set
            {
                Values[ValueKeys.DialogueText] = value;
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

        public string DependentActorId
        {
            get
            {
                if (!Values.ContainsKey(ValueKeys.DependentActorId))
                    return string.Empty;
                return Values[ValueKeys.DependentActorId];
            }
            set
            {
                Values[ValueKeys.DependentActorId] = value;
            }
        }

        public virtual bool IsDestroyed
        {
            get
            {
                if (!Stats.ContainsKey(StatKeys.IsDestroyed))
                    Stats[StatKeys.IsDestroyed] = 0;
                return Stats[StatKeys.IsDestroyed] != 0;
            }
            set
            {
                Stats[StatKeys.IsDestroyed] = value ? 1 : 0;
            }
        }

        public virtual void ChangeState(int nextState)
        {
            Stats[CurrentStateKey] = nextState;
        }

        public FlexEntity(FlexEntityDto dto)
        {
            Values = dto.Values;
            Stats = dto.Stats;
            Id = dto.Id;
        }

        public FlexEntity()
        {
            Id = Guid.NewGuid().ToString();
            Stats = new Dictionary<string, int>();
            Values = new Dictionary<string, string>();
        }

        public FlexEntity(FlexData data)
        {
            Id = Guid.NewGuid().ToString();
            Stats = new Dictionary<string, int>(data.Stats);
            Values = new Dictionary<string, string>(data.Values);
        }

        public virtual string GetLinkText()
        {
            return Name;
        }

        public virtual IRoomAction MainAction(IRoom room)
        {
            return new DoNothingAction(this);
        }

        public abstract void CalculateDialogue(IRoom room);
        
        public abstract TagString GetLookText();

        public virtual IRoomAction CleanupStep(IRoom room)
        {
            //  do nothing, but allow child classes to override this
            return new DoNothingAction(this);
        }
    }
}
