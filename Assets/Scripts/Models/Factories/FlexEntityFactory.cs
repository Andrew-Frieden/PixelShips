using System;
using Models.Dtos;
using Items;

namespace Models.Factories
{
    public static class FlexEntityFactory
    {
        public static IRoomActor FromDto(this FlexEntityDto dto)
        {
            return (IRoomActor)Activator.CreateInstance(Type.GetType(dto.EntityType), new object[] { dto });
        }
    }
}
