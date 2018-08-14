using Actions;

namespace Models
{
    public interface IRoomEntity : ITextEntity
    {
        string Description { get; }
        string GetLookText();

        ABDialogueContent GetInteraction(IRoom s);
        IRoomAction GetNextAction(IRoom s);
    }
}
