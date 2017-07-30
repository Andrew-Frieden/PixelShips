using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceActions
{
    public class SpaceActionFactory
    {
        private SpaceContext Context { get; set; }
        public SpaceActionFactory(SpaceContext context)
        {
            Context = context;
        }        

        public SpaceAction GetSpaceAction(SpaceActionDto dto)
        {
            switch (dto.Name.ToLower())
            {
                case "jump":
                    {
                        return new JumpAction(Context, dto);
                    }
                case "ping":
                    {
                        return new PingAction(Context, dto);
                    }
                case "interact":
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
