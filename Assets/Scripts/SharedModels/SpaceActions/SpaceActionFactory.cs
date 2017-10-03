using PixelSpace.Models.SharedModels.Ships;
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

            if (ship.Room == null)
                throw new Exception("Ship's room collection not initialized!");

            foreach (var exit in ship.Room.ExitIds)
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

            return actions;
        }
    }
}
