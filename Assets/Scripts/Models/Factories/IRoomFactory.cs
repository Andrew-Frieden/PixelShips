using Models;

public interface IRoomFactory
{
    IRoom GenerateRoom(RoomTemplate template);
}