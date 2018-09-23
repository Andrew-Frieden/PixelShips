using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

public interface IRoom : ITextEntity
{
    IEnumerable<RoomTemplate> Exits { get; }
    
    CommandShip PlayerShip { get; }
    string Description { get; }
    RoomFlavor Flavor { get; }
    List<IRoomActor> Entities { get; }

    void SetPlayerShip(CommandShip ship);
    void AddEntity(IRoomActor actor);
}

public static class RoomHelpers
{
    public static ITextEntity FindEntity(this IRoom room, string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        var entities = new List<ITextEntity>() { room, room.PlayerShip };
        entities.AddRange(room.Entities);

        return entities.FirstOrDefault(ent => ent.Id == id);
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