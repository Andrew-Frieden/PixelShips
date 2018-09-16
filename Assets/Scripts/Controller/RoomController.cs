using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;  // i think we should put the stat keys somewhere else - doesn't seem like roomcontroller needs to know about dtos
using Models.Stats;
using static Models.CommandShip;

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
                if (!entity.IsHidden)
                {
                    entity.DialogueContent = entity.CalculateDialogue(room);
                }
            }
            
            room.PlayerShip.DialogueContent =  room.PlayerShip.CalculateDialogue(room);
            room.DialogueContent = DialogueBuilder.PlayerNavigateDialogue(room);
        }

        private static IEnumerable<string> DoCleanup(IRoom room)
        {
            room.Entities.ForEach(e => e.AfterAction(room));
            var destroyedEntities = room.Entities.Where(e => e.IsDestroyed).ToList();
            destroyedEntities.ForEach(e => room.Entities.Remove(e));

            if (room.PlayerShip.IsDestroyed)
            {
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