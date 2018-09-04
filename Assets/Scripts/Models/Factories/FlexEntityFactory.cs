using System;
using Models.Dtos;

namespace Models.Factories
{
    public static class FlexEntityFactory
    {
        public static IRoomActor FromDto(this FlexEntityDto dto, IRoom room)
        {
            return (IRoomActor)Activator.CreateInstance(Type.GetType(dto.EntityType), new { dto, room });
        }
    }
}
