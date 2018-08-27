using System.Collections.Generic;
using Models.Actions;

namespace Models.Actors
{
    public class DelayedActor : IRoomActor
    {
        public const string TimeToLiveKey = "timetolive";

        public string Id { get; protected set; }
        public Dictionary<string, int> Stats { get; }
        public ABDialogueContent DialogueContent { get; set; }

        public DelayedActor() : base()
        {
            Stats = new Dictionary<string, int>();
        }
        
        public string GetLookText()
        {
            throw new System.NotImplementedException();
        }

        public string GetLinkText()
        {
            throw new System.NotImplementedException();
        }

        public virtual IRoomAction GetNextAction(IRoom s)
        {
            throw new System.NotImplementedException();
        }
    }
}