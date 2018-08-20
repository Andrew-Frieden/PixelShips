namespace Models
{
    public interface IRoomEntity : ITextEntity
    {
        string Description { get; }
        IRoomAction GetNextAction(IRoom s);
    }
}
