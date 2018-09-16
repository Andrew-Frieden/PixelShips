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
            if (5 > Random.Range(1, 10))
            {
                entities.Add(new SometimesDamageHazard(data.Stats, data.Values));
            }

            var miningAsteroid = new SingleUseGatherable();
            if (5 > Random.Range(1, 10))
            {
                entities.Add(miningAsteroid);
            }

            var floatingObject = new ScrapGatherable();
            if (5 > Random.Range(1, 10))
            {
                entities.Add(floatingObject);
            }

            var mobList = new List<IRoomActor>();
            mobList.Add(new PirateMob());
            mobList.Add(new VerdantObserverMob());
            mobList.Add(new VerdantInformantMob());
            mobList.Add(new VerdantInterrogatorMob());

            if (7 > Random.Range(1, 10))
            {
                var tempList = mobList.OrderBy(d => System.Guid.NewGuid());
                entities.Add(tempList.First());
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