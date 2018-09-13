using GameData;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Models.Factories.Helpers
{
    public static class RoomFactoryHelper
    {
        public static IEnumerable<IRoomActor> BuildKelpForestActors(RoomTemplate template)
        {
            var entities = new List<IRoomActor>();
            
            var exampleNpc = new NeedsHelpNpc();
            if (template.Difficulty * 10 < Random.Range(1, 100))
            {
                entities.Add(exampleNpc);
            }

            var data = ExampleGameData.SometimesDamageHazards.OrderBy(d => System.Guid.NewGuid()).First();
            entities.Add(new SometimesDamageHazard(data.Stats, data.Values));

            var miningAsteroid = new MiningGatherableObject();
            if (5 > Random.Range(1, 10))
            {
                entities.Add(miningAsteroid);
            }

            var floatingObject = new SimpleFloatingObject();
            if (5 > Random.Range(1, 10))
            {
                entities.Add(floatingObject);
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
            
            //var exampleHazard = new SometimesDamageHazard(2, 20);

            //entities.Add(exampleHazard);
            
            return entities;
        }
    }
}