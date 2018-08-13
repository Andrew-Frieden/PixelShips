using Actions;

namespace Models
{
    public abstract class Mob : IRoomEntity, ICombatEntity
    {
        public string Id { get; }
        public int Hull { get; set; }
        
        public string GetLookText()
        {
            throw new System.NotImplementedException();
        }

        public string GetLinkText()
        {
            throw new System.NotImplementedException();
        }

        public ABDialogueContent GetInteraction(IRoom s)
        {
            throw new System.NotImplementedException();
        }

        public IRoomAction GetNextAction(IRoom s)
        {
            throw new System.NotImplementedException();
        }

        public ABDialogueContent GetInteraction(CmdState s)
        {
            throw new System.NotImplementedException();
        }
    }
}