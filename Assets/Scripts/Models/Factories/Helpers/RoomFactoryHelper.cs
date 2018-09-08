using System.Collections.Generic;

namespace Models.Factories.Helpers
{
    public static class RoomFactoryHelper
    {
        public static IEnumerable<IRoomActor> BuildKelpForestActors(RoomTemplate template)
        {
            var entities = new List<IRoomActor>();
            
            var exampleNpc = new NeedsHelpNpc();
            var exampleHazard = new SometimesDamageHazard(2, 20);

            if (template.Difficulty * 10 < UnityEngine.Random.Range(1, 100))
            {
                entities.Add(exampleNpc);
            }

            if (template.Difficulty * 10 < UnityEngine.Random.Range(1, 100))
            {
                entities.Add(exampleHazard);
            }

            return entities;
        }
        
        public static IEnumerable<IRoomActor> BuildNebulaActors(RoomTemplate template)
        {
            var entities = new List<IRoomActor>();
            
            var exampleNpc = new NeedsHelpNpc();

            entities.Add(exampleNpc);
            
            return entities;
        }
        
        public static IEnumerable<IRoomActor> BuildEmptySpaceActors(RoomTemplate template)
        {
            var entities = new List<IRoomActor>();
            
            var exampleHazard = new SometimesDamageHazard(2, 20);

            entities.Add(exampleHazard);
            
            return entities;
        }
    }
}