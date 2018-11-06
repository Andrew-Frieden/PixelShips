using System;
using System.Collections.Generic;
using TextSpace.Models.Stats;

namespace TextSpace.Models
{
    public class FlexData
    {
        public string EntityType;
        public int Powerlevel;
        public IEnumerable<RoomFlavor> RoomFlavors;
        public IEnumerable<RoomActorFlavor> ActorFlavors;

        public Dictionary<string, int> Stats;
        public Dictionary<string, string> Values;
    }

    public static class FlexDataHelpers
    {
        public static IRoomActor FromFlexData(this FlexData data)
        {
            try
            {
                return (IRoomActor) Activator.CreateInstance(Type.GetType(data.EntityType), new object[] { data });
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(data);
                Console.WriteLine(data.Values[ValueKeys.Name]);
                Console.WriteLine(data.EntityType);
                throw;
            }
        }
    }
}
