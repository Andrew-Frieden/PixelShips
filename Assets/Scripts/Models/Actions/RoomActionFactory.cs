using Models.Dtos;
using System;

namespace Models.Actions
{
    public class RoomActionFactory
    {
        private IRoom _room;

        public RoomActionFactory(IRoom room)
        {
            _room = room;
        }

        public IRoomAction GetSimpleAction(SimpleActionDto dto)
        {
            switch (dto.ActionName)
            {
                case "Attack":
                    return new AttackAction(_room, dto);
                case "AnotherAttack":
                    throw new NotImplementedException();
            }

            throw new Exception("RoomActionFactory.GetSimpleAction() => could not build the action!");
        }
    }
}