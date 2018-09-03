using System.Collections.Generic;
 using Models.Actions;

namespace Models
{
    public interface IRoomActor : ITextEntity
    {
        Dictionary<string, int> Stats { get; }
        //Dictionary<string, string> Text { get; }

        IRoomAction GetNextAction(IRoom room);

        void AfterAction(IRoom room);
        void ChangeState(int nextState);
        ABDialogueContent CalculateDialogue(IRoom room);
    }
}
