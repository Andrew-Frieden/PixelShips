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

            return entities;
        }
    }
}