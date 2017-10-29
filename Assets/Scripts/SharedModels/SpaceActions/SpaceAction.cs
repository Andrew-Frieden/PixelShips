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

        //  this represents the time the action was seen by the universe
        public DateTime DateCreated { get; set; }

        protected IRoomState State { get; set; }
        protected SpaceAction() { }
        public SpaceAction(IRoomState state, SpaceActionDbi dbi)
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

                if (State.Room.Id != dbi.SourceRoomId)
                    throw new Exception("SpaceAction BuildFromContext State room id does not match space action's source room");

                Id = dbi.Id;
                Version = dbi.Version;
                SourceRoom = State.Room;
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
