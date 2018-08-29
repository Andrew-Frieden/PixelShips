using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Actions;
using UnityEngine;

public interface IRoom : ITextEntity
{
    int _tick { get; }
    void Tick();
    
    List<RoomTemplate> RoomTemplates { get; set; }
    
    CommandShip PlayerShip { get; }
    string Description { get; set; }
    RoomFlavor Flavor { get; }
    List<IRoomActor> Entities { get; }
    List<IRoom> Exits { set; get; }

    List<string> ResolveNext(IRoomAction playerAction);

    void SetPlayerShip(CommandShip ship);
    void AddEntity(IRoomActor actor);
}

public static class RoomHelpers
{
    public static ITextEntity FindEntity(this IRoom room, string id)
    {
        return room.Entities.FirstOrDefault(e => e.Id == id) ?? (ITextEntity)room.PlayerShip;
    }
    
    public static IRoomActor FindActor(this IRoom room, string id)
    {
        var entity = room.Entities.FirstOrDefault(e => e.Id == id) ?? (ITextEntity) room.PlayerShip;
        if (entity is IRoomActor)
        {
            return (IRoomActor) entity;
        }
        else
        {
            Debug.Log($"Error: Cannot find actor by id: {id}");
            return null;
        }
    }
}