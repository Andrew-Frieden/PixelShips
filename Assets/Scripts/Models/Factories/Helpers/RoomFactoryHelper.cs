using GameData;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Models.RoomEntities.Mobs;

namespace Models.Factories.Helpers
{
    public static class RoomFactoryHelper
    {
        public static IEnumerable<IRoomActor> BuildKelpForestActors(RoomTemplate template)
        {

            // 0-1 NPC
            // 0-2 Gatherable
            // 0-2 Damage Sources
            var entities = new List<IRoomActor>();

            var NPCBudget = Random.Range(0, 1);
            var GatherableBudget = Random.Range(0, 2);
            var DamageBudget = Random.Range(0, 2);
            var hasHazard = false;
            var hasBoss = false;
            
            var exampleNpc = new NeedsHelpNpc();
            if (NPCBudget > 0)
            {
                entities.Add(exampleNpc);
                NPCBudget--;
            }

            while (GatherableBudget > 0)
            {
                var miningAsteroid = new SingleUseGatherable();
                var floatingObject = new ScrapGatherable();
                if (5 > Random.Range(1, 10))
                {
                    entities.Add(miningAsteroid);
                } else
                {
                    entities.Add(floatingObject);
                }
                GatherableBudget--;

            }

            while (DamageBudget > 0)
            {

                var randomCount = Random.Range(1, 10);
                if ((3 < randomCount) && !hasHazard)
                {
                    var data = InjectableGameData.SometimesDamageHazards.OrderBy(d => System.Guid.NewGuid()).First();
                    entities.Add(new SometimesDamageHazard(data));
                    hasHazard = true;

                } else if ((5 < randomCount) && !hasBoss)
                {
                    entities.Add(new VerdantInterrogatorMob());
                    hasBoss = true;
                } else
                {
                    var mobList = new List<IRoomActor>();
                    mobList.Add(new PirateMob());
                    mobList.Add(new VerdantObserverMob());
                    mobList.Add(new VerdantInformantMob());

                    var tempList = mobList.OrderBy(d => System.Guid.NewGuid());
                    entities.Add(tempList.First());

                }
                DamageBudget--;
            }

            if (Random.Range(1,101) <= 25)
                entities.Add(new SpaceStationNpc());

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