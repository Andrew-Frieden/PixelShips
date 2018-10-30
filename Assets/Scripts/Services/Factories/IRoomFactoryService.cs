using TextSpace.Models;

public interface IRoomFactory
{
    IRoom GenerateRoom(RoomTemplate template);
}