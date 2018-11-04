using TextSpace.Models.Dtos;
using System.Collections.Generic;

namespace TextSpace.Models
{
    public class RoomTemplate
    {
        public int PowerLevel { get; }
        public RoomFlavor Flavor { get; }
        public IEnumerable<RoomActorFlavor> ActorFlavors { get; }

        public RoomTemplate(int powerlevel, RoomFlavor flavor, IEnumerable<RoomActorFlavor> actorFlavors)
        {
            PowerLevel = powerlevel;
            Flavor = flavor;
            ActorFlavors = actorFlavors;
        }

        public RoomTemplate(int powerlevel, RoomFlavor flavor)
        {
            PowerLevel = powerlevel;
            Flavor = flavor;
            ActorFlavors = new RoomActorFlavor[0];
        }

        public RoomTemplate(RoomTemplateDto dto)
        {
            PowerLevel = dto.PowerLevel;
            Flavor = dto.Flavor;
            ActorFlavors = dto.ActorFlavors;
        }
    }

    public enum RoomActorFlavor
    {
        None = 0,
        Mob = 1,
        Hazard = 2,
        Gatherable = 3,
        Town = 4,   //  these are npcs but usually/always have repair and trade actions
        Npc = 5,    //  might eventually want to break this up into more specific kinds of npcs
        Mineable = 6,
        Mission = 7
    }
}