using System.Collections.Generic;
using Models;

public interface IRoom : ITextEntity
{
    CommandShip PlayerShip { get; }
    string Description { get; }
    RoomFlavor Flavor { get; }
    List<IRoomActor> Entities { get; }

    List<string> ResolveNext(IRoomAction playerAction);

    void SetPlayerShip(CommandShip ship);
    void AddEntity(IRoomActor actor);
}