using System.Collections.Generic;

namespace Models.Actions
{
    public interface IRoomAction
    {
        IEnumerable<TagString> Execute(IRoom room);

        bool IsValid();
    }
}