using System.Collections.Generic;
 using Models.Actions;

namespace Models
{
    public interface IRoomActor : ITextEntity
    {
        bool PrintToScreen { get; set; }
        bool IsAggro { get; set; }
        bool CanCombat { get; }
        Dictionary<string, int> Stats { get; }
        //Dictionary<string, string> Text { get; }

        IRoomAction GetNextAction(IRoom room);

        void AfterAction(IRoom room);
        void ChangeState(int nextState);
        ABDialogueContent CalculateDialogue(IRoom room);
    }
}
