using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dtos;
using UnityEngine;
using Models.Dialogue;

namespace Models
{
    public abstract class FlexEntity : IRoomActor
    {
        public bool IsCombatFlagged { get; set; }   //  if this is true, player interactions are all combat related. Only the Entity should be able to switch this.
        
        public int Hull { get; set; }   //  expect to have combat stats in Stats for any FlexEntity that IsCombatFlagged
        
        public string Id { get; protected set; }
        public string Description { get; }  //  do we really need a description field? shouldn't GetLookText calculate it?
        public string Name { get; protected set; }
        
        public Dictionary<string, int> Stats { get; private set; }
        public ABDialogueContent DialogueContent { get; set; }

        const string CurrentStateKey = "current_state"; 
        protected int CurrentState 
        { 
            get
            {
                if (Stats == null)
                    Stats = new Dictionary<string, int>();
                    
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

    public class ExampleMobFlexEntity
    {
    }
    
    public class ExampleHazardFlexEntity
    {
    }
}
