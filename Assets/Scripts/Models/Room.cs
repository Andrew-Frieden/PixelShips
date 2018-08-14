using System.Collections.Generic;
using Models;
using System.Linq;

namespace Models
{
    public class Room : IRoom
    {
        public CommandShip PlayerShip { get; }
        public string Description { get; }
        public RoomFlavor Flavor { get; }
        public List<IRoomEntity> Entities { get; }

        public Room(CommandShip ship, List<IRoomEntity> roomEntities)
        {
            PlayerShip = ship;
            Entities = roomEntities;
        }

        public List<string> ResolveNext(IRoomAction playerAction)
        {
            var actions = new List<IRoomAction>() {playerAction};
            Entities.ForEach(e => actions.Add(e.GetNextAction(this)));

            var resultText = new List<string>();
            actions.ForEach(a => resultText.AddRange(a.Execute(this)));

            return resultText;
        }
    }
}
    
