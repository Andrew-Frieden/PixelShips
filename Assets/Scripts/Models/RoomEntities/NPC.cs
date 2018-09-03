using System.Collections.Generic;
using Models.Actions;

namespace Models
{
    public class NPC : IRoomActor
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

        public void AfterAction(IRoom room) { }

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