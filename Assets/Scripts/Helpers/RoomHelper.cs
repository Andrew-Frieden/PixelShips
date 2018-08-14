using System.Linq;
using Models;

namespace Helpers
{
    public static class RoomHelper
    {
        public static ITextEntity FindTextEntityByGuid(IRoom room, string guid)
        {
            return room.Entities.SingleOrDefault(e => e.Id == guid) ?? (ITextEntity) room.PlayerShip;
        }
    }
}