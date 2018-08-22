namespace Models
{
    public interface IRoomActor : ITextEntity
    {
        IRoomAction GetNextAction(IRoom s);
    }
}
