namespace Models.Actors
{
    public class DelayedActor : IRoomActor
    {
        protected int _timeToLive;
        public string Id { get; }
        public ABDialogueContent DialogueContent { get; }

        public void Tick()
        {
            _timeToLive -= _timeToLive;
        }
        
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