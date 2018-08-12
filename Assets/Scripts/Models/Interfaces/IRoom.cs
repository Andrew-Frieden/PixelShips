using System.Collections.Generic;

public interface IRoom
{
    string Description { get; }
    RoomFlavor Flavor { get; }
    List<IRoomEntity> Entities { get; }
}