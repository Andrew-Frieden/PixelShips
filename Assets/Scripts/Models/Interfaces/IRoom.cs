using System.Collections.Generic;
using System.Linq;
using TextSpace.Models;
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
    void CalculateDialogue();
}

public static class RoomHelpers
{
    public static ITextEntity FindEntity(this IRoom room, string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        //  include all the places we should look for entities
        var entities = new List<ITextEntity>() { room, room.PlayerShip };
        entities.AddRange(room.Entities);
        entities.AddRange(room.PlayerShip.Hardware);
        entities.Add(room.PlayerShip.LightWeapon);
        entities.Add(room.PlayerShip.HeavyWeapon);

        return entities.FirstOrDefault(ent => ent.Id == id);
    }

    public static ICollection<IRoomActor> FindDependentActors(this IRoom room, string parentId)
    {
        return room.Entities.Where(e => e.DependentActorId == parentId).ToList();
    }

    public static ICollection<IRoomActor> FindDependentActors(this IRoom room, IRoomActor parent)
    {
        return FindDependentActors(room, parent.Id);
    }
}