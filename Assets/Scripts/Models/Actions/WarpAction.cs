using System.Collections.Generic;

namespace Models.Actions
{
    public class WarpAction : IRoomAction
    {
        private IRoom _room;
        
        public WarpAction(IRoom room)
        {
            _room = room;
        }
        
        public IEnumerable<string> Execute(IRoom room)
        {
            //Call command view controller
            
            
            
            return new List<string>() { "3...", "2...", "1..." };
        }
    }
}