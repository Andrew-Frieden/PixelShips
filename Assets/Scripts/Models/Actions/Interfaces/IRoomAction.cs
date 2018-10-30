using System.Collections.Generic;

namespace TextSpace.Models.Actions
{
    public interface IRoomAction
    {
        IEnumerable<TagString> Execute(IRoom room);
        void CalculateValid(IRoom room);
        bool IsValid { get; set; }
    }
}