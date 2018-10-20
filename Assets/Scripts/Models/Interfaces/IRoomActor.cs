using System.Collections.Generic;
using Items;
using Models.Actions;

namespace Models
{
    public interface IRoomActor : ITextEntity
    {
        string DependentActorId { get; set; }
        
        bool IsDestroyed { get; set; }
        bool IsHidden { get; set; }
        bool IsHostile { get; set; }
        bool IsAttackable { get; set; }

        Dictionary<string, int> Stats { get; }
        Dictionary<string, string> Values { get; }

        IRoomAction MainAction(IRoom room);
        IRoomAction CleanupStep(IRoom room);

        void ChangeState(int nextState);
        void CalculateDialogue(IRoom room);
    }
}
