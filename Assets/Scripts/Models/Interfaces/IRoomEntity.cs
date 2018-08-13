using Actions;

namespace Models
{
    public interface IRoomEntity : ITextEntity
    {
        string GetLookText();

        ABDialogueContent GetInteraction(IRoom s);
        IRoomAction GetNextAction(IRoom s);
    }
}
