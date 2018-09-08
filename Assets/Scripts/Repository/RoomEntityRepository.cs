using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Repository
{
    public class RoomEntityRepository : IJsonRepository<IRoomActor>
    {
        public IEnumerable<IRoomActor> LoadData()
        {
            var entities = new List<IRoomActor>();

            var exampleNpc = new ExampleNpcFlexEntity();
            var exampleHazard = new ExampleHazardFlexEntity(2, 80);

            entities.Add(exampleNpc);
            entities.Add(exampleHazard);

            return entities;
        }
    }
}