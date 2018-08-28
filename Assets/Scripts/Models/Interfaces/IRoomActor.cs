using System.Collections.Generic;
 using Models.Actions;

namespace Models
{
    public interface IRoomActor : ITextEntity
    {
        Dictionary<string, int> Stats { get; }

        IRoomAction GetNextAction(IRoom s);

        void AfterAction(IRoom room);
    }
}
