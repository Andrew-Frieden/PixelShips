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

        //  does this represent the time the action was created or when it occurs in the universe?
        //  this should probably represent the time the action was seen by the universe
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
            try
            {
                if (dbi.Name != this.Name)
                {
                    throw new InvalidOperationException("tried to construct the wrong kind of space action");
                }

                Id = dbi.Id;
                Version = dbi.Version;


                SourceRoom = State.Rooms.Single(r => r.Id == dbi.SourceRoomId);
            }
            catch(Exception e)
            {
                throw new Exception("SpaceAction broke trying to build " + e.Message);
            }
        }

        public abstract bool Validate();
        public abstract IEnumerable<SpaceActionDbi> Execute();

        public abstract SpaceActionDbi ToDbi();
    }
}
