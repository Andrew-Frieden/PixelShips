using System.Collections.Generic;
using Models.Factories;
using Controller;

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
            room.PlayerShip.WarpDriveReady = false;
            room.PlayerShip.WarpTarget = _template;
            
            return new List<string>() { "You begin to jump into hyperspace!", "3...", "2...", "1..." };
        }
    }
}