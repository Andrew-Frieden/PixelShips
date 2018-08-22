using System;
namespace Models
{
    public class SaveState
    {
        public DateTime SaveTime;
        public RoomDto Room;
    }
    
    public class GameState
    {
        public DateTime CurrentTime;
        public Room Room;
    }
}
