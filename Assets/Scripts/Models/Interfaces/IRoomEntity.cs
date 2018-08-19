using Actions;
using Models.Text;

namespace Models
{
    public interface IRoomEntity : ITextEntity
    {
        string Description { get; }

        ABDialogueContent GetInteraction(IRoom s);
        IRoomAction GetNextAction(IRoom s);
    }
}
