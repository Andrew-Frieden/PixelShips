using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceActions
{
    public class SpaceActionFactory
    {
        private ISpaceState State { get; set; }
        public SpaceActionFactory(ISpaceState state)
        {
            State = state;
        }        

        public SpaceAction GetModel(SpaceActionDbi dbi)
        {
            switch (dbi.Name.ToLower())
            {
                case "jump":
                    {
                        return new JumpAction(State, dbi);
                    }
                case "ping":
                    {
                        return new PingAction(State, dbi);
                    }
                case "interact":
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
