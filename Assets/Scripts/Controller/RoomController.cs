using System.Collections.Generic;
using Models.Actions;

namespace Controller
{
    public sealed class RoomController
    {
        public void StartNextRoom(IRoom nextRoom, IRoom previousRoom)
        {
            nextRoom.SetPlayerShip(previousRoom.PlayerShip);
            previousRoom.PlayerShip.WarpDriveReady = false;
            nextRoom.PlayerShip.DialogueContent =  nextRoom.PlayerShip.CalculateDialogue(nextRoom);
            
            foreach(var ent in nextRoom.Entities)
            {
                if (ent != nextRoom.PlayerShip)
                {
                    ent.DialogueContent = ent.CalculateDialogue(nextRoom);
                }
            }
        }

        public IEnumerable<string> ResolveNextTick(IRoom room, IRoomAction playerAction)
        {
            var resolveText = ExecuteActions(room, playerAction);

            DoCleanup(room);
            CalculateNewDialogues(room);
            room.Tick();

            return resolveText;
        }

        private IEnumerable<string> ExecuteActions(IRoom room, IRoomAction playerAction)
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

        private void CalculateNewDialogues(IRoom room)
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

        private void DoCleanup(IRoom room)
        {
            foreach (var entity in room.Entities)
            {
                //entity.AfterAction(_room);
            }
        }
    }
}