using System.Collections.Generic;
using Models;

public interface IRoom
{
    Ship PlayerShip { get; }
    string Description { get; }
    RoomFlavor Flavor { get; }
    List<IRoomEntity> Entities { get; }
}