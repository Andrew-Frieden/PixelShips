using System.Collections.Generic;

namespace Models
{
    public interface IRoomActor : ITextEntity
    {
        Dictionary<string, int> Stats { get; }

        IRoomAction GetNextAction(IRoom s);
    }
}
