using System;
using System.Collections.Generic;

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
            return (IRoomActor)Activator.CreateInstance(Type.GetType(data.EntityType), new object[] { data });
        }
    }
}
