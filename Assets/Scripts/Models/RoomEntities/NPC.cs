namespace Models
{
    public class NPC : IRoomActor
    {
        public string Id { get; }
        public ABDialogueContent DialogueContent { get; }
        public string Description { get; }
        
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