using System.Collections.Generic;
using Models.Actions;

namespace Models.Actors
{
    public abstract class DelayedActor : IRoomActor
    {
        public const string TimeToLiveKey = "timetolive";

        public string Id { get; protected set; }
        public Dictionary<string, int> Stats { get; }
        public ABDialogueContent DialogueContent { get; set; }

        public bool IsAggro
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
                throw new System.NotImplementedException();
            }
        }

        public bool CanCombat
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
                throw new System.NotImplementedException();
            }
        }

        public abstract string GetLookText();
        public abstract string GetLinkText();
        public abstract IRoomAction GetNextAction(IRoom s);

        protected DelayedActor()
        {
            Stats = new Dictionary<string, int>();
        }

        public void AfterAction(IRoom room)
        {
            if (Stats[TimeToLiveKey] == 0)
            {
                room.Entities.Remove(this);
            }
        }

        public void ChangeState(int nextState)
        {
            throw new System.NotImplementedException();
        }

        public ABDialogueContent CalculateDialogue(IRoom room)
        {
            throw new System.NotImplementedException();
        }
    }
}