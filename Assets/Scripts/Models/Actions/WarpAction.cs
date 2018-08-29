using System.Collections.Generic;

namespace Models.Actions
{
    public class WarpAction : IRoomAction
    {
        public IEnumerable<string> Execute(IRoom room)
        {
            return new List<string>() { "You warp to the next room!" };
        }
    }
}