using Actions;
using Models.Text;

namespace Models
{
    public interface IRoomEntity : ITextEntity
    {
        string Description { get; }
        TextBlock GetLookText();

        ABDialogueContent GetInteraction(IRoom s);
        IRoomAction GetNextAction(IRoom s);
    }
}
