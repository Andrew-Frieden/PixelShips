using System;
using TextSpace.Models;
using TextSpace.Models.Dtos;

namespace TextSpace.Services.Factories
{
    public static class RoomFactoryHelper
    {
        public static IRoomActor FromFlexData(this FlexData data)
        {
            return (IRoomActor)Activator.CreateInstance(Type.GetType(data.EntityType), new object[] { data });
        }
        
        public static IRoomActor FromDto(this FlexEntityDto dto)
        {
            return (IRoomActor)Activator.CreateInstance(Type.GetType(dto.EntityType), new object[] { dto });
        }
    }
}