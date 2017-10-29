using PixelSpace.Models.SharedModels.Ships;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceActions
{
    public class SpaceActionFactory
    {
        private IRoomState State { get; set; }
        public SpaceActionFactory(IRoomState state)
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
                case "start_instant_jump":
                    {
                        return new StartInstantJumpAction(State, dbi);
                    }
                case "end_instant_jump":
                    {
                        return new EndInstantJumpAction(State, dbi);
                    }
                case "interact":
                default:
                    throw new NotImplementedException();
            }
        }

        public IEnumerable<SpaceAction> GetPossibleActionsForShip(Ship ship)
        {
            var actions = new List<SpaceAction>();

            //  get a ping action
            //var pingDbi = new SpaceActionDbi()
            //{
            //    Name = "ping",
            //    SourceId = ship.Id,
            //    SourceType = "ship",
            //    TargetId = null,
            //    SourceRoomId = ship.RoomId
            //};
            //actions.Add(GetModel(pingDbi));

            //  get all jump actions
            if (State.Room != null)
            {
                foreach (var exit in State.Room.ExitIds)
                {
                    var jumpDbi = new SpaceActionDbi()
                    {
                        Name = "start_instant_jump",
                        SourceId = ship.Id,
                        SourceType = "ship",
                        SourceRoomId = ship.RoomId,
                        TargetId = exit
                    };
                    actions.Add(GetModel(jumpDbi));
                }
            }
            else
            {
            }

            return actions;
        }
    }
}
