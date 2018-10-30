using System;
using TextSpace.Models;
using TextSpace.Models.Actions;
using TextSpace.Models.Dtos;

namespace TextSpace.Services.Factories
{
    public static class RoomFactoryHelper
    {
        public static IRoomActor FromFlexData(this FlexData data)
        {
            return (IRoomActor)Activator.CreateInstance(Type.GetType(data.EntityType), new object[] { data });
        }
        
        public static IRoomAction FromDto(this SimpleActionDto dto, IRoom _room)
        {
            return (IRoomAction)Activator.CreateInstance(Type.GetType(dto.ActionType), new object[] { dto, _room });
        }
        
        public static IRoomActor FromDto(this FlexEntityDto dto)
        {
            return (IRoomActor)Activator.CreateInstance(Type.GetType(dto.EntityType), new object[] { dto });
        }
    }
}