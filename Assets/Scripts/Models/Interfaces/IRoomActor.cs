using System.Collections.Generic;
 using Models.Actions;

namespace Models
{
    public interface IRoomActor : ITextEntity
    {
        bool IsHidden { get; }
        bool IsHostile { get; set; }
        bool IsAttackable { get; set; }

        Dictionary<string, int> Stats { get; }
        Dictionary<string, string> Values { get; }

        IRoomAction GetNextAction(IRoom room);

        void AfterAction(IRoom room);
        void ChangeState(int nextState);
        ABDialogueContent CalculateDialogue(IRoom room);
    }
}
