using System.Collections.Generic;
 using Models.Actions;

namespace Models
{
    public interface IRoomActor : ITextEntity
    {
        bool Hidden { get; }
        bool IsAggro { get; set; }
        bool CanCombat { get; }
        Dictionary<string, int> Stats { get; }
        Dictionary<string, string> Values { get; }

        IRoomAction GetNextAction(IRoom room);

        void AfterAction(IRoom room);
        void ChangeState(int nextState);
        ABDialogueContent CalculateDialogue(IRoom room);
    }
}