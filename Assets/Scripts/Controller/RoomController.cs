using System.Collections.Generic;
using System.Linq;
using Models.Actions;
using Models.Dtos;
using Models.Stats;

namespace Controller
{
    public static class RoomController
    {
        public static void StartNextRoom(IRoom nextRoom, IRoom previousRoom)
        {
            nextRoom.SetPlayerShip(previousRoom.PlayerShip);
            nextRoom.PlayerShip.WarpTarget = null;
            CalculateDialogues(nextRoom);
        }

        public static IEnumerable<string> ResolveNextTick(IRoom room, IRoomAction playerAction)
        {
            var resolveText = (List<string>) ExecuteActions(room, playerAction);
            var cleanupText = DoCleanup(room);

            if (cleanupText.Any())
            {
                resolveText.AddRange(cleanupText);
            }
            else
            {
                CalculateDialogues(room);
                room.Tick();
            }

            return resolveText;
        }

        private static IEnumerable<string> ExecuteActions(IRoom room, IRoomAction playerAction)
        {
            var actionResults = new List<string>();
            var actionsToExecute = new List<IRoomAction>()
            {
                playerAction
            };

            //TODO: Dont love that this business logic is in here 
            //The point of this is that if we are warping we don't execute anything else
            //for that tick. We could change this flow.
            if (!(playerAction is WarpAction))
            {
                foreach (var entity in room.Entities)
                {
                    var nextAction = entity.GetNextAction(room);
                    actionsToExecute.Add(nextAction);
                }
            }
            
            foreach (var action in actionsToExecute)
            {
                actionResults.AddRange(action.Execute(room));
            }

            return actionResults;
        }

        private static void CalculateDialogues(IRoom room)
        {
            foreach (var entity in room.Entities)
            {
                if (!entity.Hidden)
                {
                    entity.DialogueContent = entity.CalculateDialogue(room);
                }
            }
            
            room.PlayerShip.DialogueContent =  room.PlayerShip.CalculateDialogue(room);
        }

        private static IEnumerable<string> DoCleanup(IRoom room)
        {
            foreach (var entity in room.Entities)
            {
                //entity.AfterAction(_room);
            }

            if (room.PlayerShip.Stats[StatKeys.Hull] < 1)
            {
                room.PlayerShip.Stats[ShipDto.StatKeys.IsAlive] = 0;
                return new List<string>
                {
                    "You have been destroyed.",
                    "Navigate to your base to recruit a new captain."
                };
            }

            return new List<string>();
        }
    }
}