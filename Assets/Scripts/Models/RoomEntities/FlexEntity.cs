using System.Collections.Generic;
using Models.Actions;
using Models.Dtos;

namespace Models
{
    public abstract class FlexEntity : IRoomActor
    {
        public bool PrintToScreen { get; set; }

        public bool IsAggro
        {
            get
            {
                return Stats[StatKeys.IsAggro] != 0;
            }
            set
            {
                Stats[StatKeys.IsAggro] = value ? 1 : 0;
            }
        }

        public bool CanCombat
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
        
        public string Id { get; protected set; }
        public string Description { get; }  //  do we really need a description field? shouldn't GetLookText calculate it?
        public string Name { get; protected set; }
        
        public Dictionary<string, int> Stats { get; protected set; }
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

        public void ChangeState(int nextState)
        {
            Stats[CurrentStateKey] = nextState;
        }

        protected FlexEntity(FlexEntityDto dto, IRoom room)
        {
            Stats = dto.Stats;
            Id = dto.Id;
            Name = dto.Name;
        }

        protected FlexEntity() { }
        
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

    public static class StatKeys
    {
        public static readonly string CanCombat = "can_combat";     //  1 if the entity is capable of participating in combat
        public static readonly string IsAggro = "is_aggro";         //  1 if the entity is actively attacking or being attacked
        public static readonly string Hull = "current_hull";
        public static readonly string MaxHull = "max_hull";
        public static readonly string Captainship = "captainship";
        public static readonly string Resourcium = "resourcium";
        public static readonly string ExampleDamageMitigationStat = "damage_mitigation";
        public static readonly string WarpDriveReady = "warp_drive_ready";
    }

    public static class StatsHelper
    {
        public static Dictionary<string, int> EmptyStatsBlock()
        {
            return new Dictionary<string, int>
            {
                { StatKeys.IsAggro, 0 },
                { StatKeys.CanCombat, 0 },
                { StatKeys.Hull, 0 },
                { StatKeys.Captainship, 0 },
            };
        }
    }
}
