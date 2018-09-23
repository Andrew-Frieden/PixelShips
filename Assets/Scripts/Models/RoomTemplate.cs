using System.Collections.Generic;

namespace Models
{
    public class RoomTemplate
    {
        public int Difficulty { get; }
        public RoomFlavor Flavor { get; }
        public IEnumerable<RoomActorFlavor> ActorFlavors { get; }

        public RoomTemplate(int difficulty, RoomFlavor flavor, IEnumerable<RoomActorFlavor> actorFlavors)
        {
            Difficulty = difficulty;
            Flavor = flavor;
            ActorFlavors = actorFlavors;
        }

        public RoomTemplate(int difficulty, RoomFlavor flavor)
        {
            Difficulty = difficulty;
            Flavor = flavor;
            ActorFlavors = new RoomActorFlavor[0];
        }
    }

    public enum RoomActorFlavor
    {
        None = 0,
        Mob = 1,
        Hazard = 2,
        Gatherable = 3,
        Town = 4,   //  these are npcs but usually/always have repair and trade actions
        Npc = 5     //  might eventually want to break this up into more specific kinds of npcs
    }
}