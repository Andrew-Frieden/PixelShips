using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels
{
    public abstract class SpaceAction
    {
        public abstract string Name { get; }
        public Room SourceRoom { get; set; }
        public DateTime DateCreated { get; set; }

        protected SpaceContext Context { get; set; }
        protected SpaceAction() { }
        public SpaceAction(SpaceContext context, SpaceActionDto dto)
        {
            Context = context;
            BuildFromContext(dto);
        }

        protected virtual void BuildFromContext(SpaceActionDto dto)
        {
            if (dto.Name != this.Name)
            {
                throw new InvalidOperationException("tried to construct the wrong kind of space action");
            }

            SourceRoom = Context.Rooms.Single(r => r.Id == dto.SourceRoomId);
        }

        public abstract bool Validate();
        public abstract void Execute();

        public abstract SpaceActionDto ToDto();
    }
}
