using System.Collections.Generic;
using Models.Actions;

namespace Models
{
    public class Hazard : IRoomActor
    {
        public string Id { get; }
        public ABDialogueContent DialogueContent { get; set; }
        public string Description { get; }
        public Dictionary<string, int> Stats { get; }
        
        public string GetLookText()
        {
            throw new System.NotImplementedException();
        }

        public string GetLinkText()
        {
            throw new System.NotImplementedException();
        }

        public IRoomAction GetNextAction(IRoom s)
        {
            throw new System.NotImplementedException();
        }
    }
}