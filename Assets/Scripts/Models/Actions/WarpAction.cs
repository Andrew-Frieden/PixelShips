using System.Collections.Generic;
using Models.Factories;
using Repository;

namespace Models.Actions
{
    public class WarpAction : IRoomAction
    {
        private readonly RoomTemplate _template;
        
        public WarpAction(RoomTemplate template)
        {
            _template = template;
        }
        
        public IEnumerable<string> Execute(IRoom room)
        {
            room.Exit = FactoryContainer.RoomFactory.GenerateRoom(_template);
            
            return new List<string>() { "You begin to jump into hyperspace!", "3...", "2...", "1..." };
        }
    }
}