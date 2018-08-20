using System.Collections.Generic;
using Models;

namespace Repository
{
    public class RoomRepository : IJsonRepository<IRoom>
    {
        public IEnumerable<IRoom> LoadData()
        {
            var rooms = new List<IRoom>();
            
            var giantNebulaContent = new ABDialogueContent()
            {
                MainText = @"This sector seems to be a dizzying array of star dust and deadly asteroids.",
                OptionAText = "Warp to next sector",
                OptionBText = "This should just be an A or cancel I think"
            };
            
            rooms.Add(new Room("You enter into a {{ link }} with many asteroids.", "Giant Nebula", null, new List<IRoomEntity>() { }, giantNebulaContent));
            
            //TODO: Remove commandship and entities from constructor and add them post content creation so the repository isn't dependant on game state

            return rooms;
        }
    }
}