using System.Collections.Generic;

namespace Models.Actions
{
    public interface IRoomAction
    {
        IEnumerable<StringTagContainer> Execute(IRoom room);
    }
}