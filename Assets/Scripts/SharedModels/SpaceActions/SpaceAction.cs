using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels
{
    public abstract class SpaceAction
    {
        public string Id { get; set; }
        public int? Version { get; set; }

        public abstract string Name { get; }
        public Room SourceRoom { get; set; }
        public DateTime DateCreated { get; set; }

        protected ISpaceState State { get; set; }
        protected SpaceAction() { }
        public SpaceAction(ISpaceState state, SpaceActionDbi dbi)
        {
            State = state;
            BuildFromContext(dbi);
        }

        protected virtual void BuildFromContext(SpaceActionDbi dbi)
        {
            if (dbi.Name != this.Name)
            {
                throw new InvalidOperationException("tried to construct the wrong kind of space action");
            }

            Id = dbi.Id;
            Version = dbi.Version;

            SourceRoom = State.Rooms.Single(r => r.Id == dbi.SourceRoomId);
        }

        public abstract bool Validate();
        public abstract IEnumerable<SpaceAction> Execute();

        public abstract SpaceActionDbi ToDbi();
    }
}
