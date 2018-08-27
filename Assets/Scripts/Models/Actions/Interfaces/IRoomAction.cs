using System.Collections.Generic;

namespace Models.Actions
{
    public interface IRoomAction
    {
        IEnumerable<string> Execute(IRoom room);
    }
}