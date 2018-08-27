using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Actions;

public interface IRoom : ITextEntity
{
    int _tick { get; }
    void Tick();
    
    CommandShip PlayerShip { get; }
    string Description { get; }
    RoomFlavor Flavor { get; }
    List<IRoomActor> Entities { get; }

    List<string> ResolveNext(IRoomAction playerAction);

    IRoomActor FindRoomActorByGuid(string id);

    void SetPlayerShip(CommandShip ship);
    void AddEntity(IRoomActor actor);
}

public static class RoomHelpers
{
    public static ITextEntity FindEntity(this IRoom room, string id)
    {
        return room.Entities.FirstOrDefault(e => e.Id == id) ?? (ITextEntity)room.PlayerShip;
    }
}