using System.Collections.Generic;
using Models;

public interface IRoom : ITextEntity
{
    CommandShip PlayerShip { get; }
    string Description { get; }
    RoomFlavor Flavor { get; }
    List<IRoomEntity> Entities { get; }

    List<string> ResolveNext(IRoomAction playerAction);

    void SetPlayerShip(CommandShip ship);
    void AddEntity(IRoomEntity entity);
}