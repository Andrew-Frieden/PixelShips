using System;
using System.Collections.Generic;

namespace Models.Factories
{
    public static class RoomFactoryHelper
    {
        public static IRoomActor FromFlexData(this FlexData data)
        {
            return (IRoomActor)Activator.CreateInstance(Type.GetType(data.EntityType), new object[] { data });
        }
    }
}