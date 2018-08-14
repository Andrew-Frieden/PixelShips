using System.Collections.Generic;
using Models;

public interface IRoom
{
    CommandShip PlayerShip { get; }
    string Description { get; }
    RoomFlavor Flavor { get; }
    List<IRoomEntity> Entities { get; }
}