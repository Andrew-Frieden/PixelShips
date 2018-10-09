using Models.Actions;
using Models.Dtos;
using System;

namespace Models.Factories
{    
    public static class ActionFactory
    {
        public static IRoomAction DoNothingAction(this IRoomActor source)
        {
            return new DoNothingAction(source);
        }
    
        public static IRoomAction FromDto(this SimpleActionDto dto, IRoom _room)
        {
            return (IRoomAction)Activator.CreateInstance(Type.GetType(dto.ActionType), new object[] { dto, _room });
        }
    }
}