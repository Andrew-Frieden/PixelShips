using System.Collections.Generic;
using Models.Actions;

namespace Controller
{
    public sealed class RoomController
    {
        public void StartNextRoom(IRoom nextRoom, IRoom previousRoom)
        {
            nextRoom.SetPlayerShip(previousRoom.PlayerShip);
            nextRoom.PlayerShip.DialogueContent =  nextRoom.PlayerShip.CalculateDialogue(nextRoom);
            
            foreach(var ent in nextRoom.Entities)
            {
                if (ent != nextRoom.PlayerShip)
                {
                    ent.DialogueContent = ent.CalculateDialogue(nextRoom);
                }
            }
        }

        public IEnumerable<string> ExecuteActions(IRoomAction playerAction, IRoom room)
        {
            var actionResults = new List<string>();
            var actionsToExecute = new List<IRoomAction>();

            if (!(playerAction is WarpAction))
            {
                actionsToExecute.Add(playerAction);
            }

            foreach (var entity in room.Entities)
            {
                var nextAction = entity.GetNextAction(room);
                actionsToExecute.Add(nextAction);
            }
            
            if (playerAction is WarpAction)
            {
                actionsToExecute.Add(playerAction);
            }
            
            foreach (var action in actionsToExecute)
            {
                actionResults.AddRange(action.Execute(room));
            }

            return actionResults;
        }

        public void CalculateNewDialogues(IRoom room)
        {
            foreach (var entity in room.Entities)
            {
                if (entity != room.PlayerShip && entity.PrintToScreen)
                {
                    entity.DialogueContent = entity.CalculateDialogue(room);
                }
            }
            
            room.PlayerShip.DialogueContent =  room.PlayerShip.CalculateDialogue(room);
        }

        public void DoCleanup(IRoom room)
        {
            foreach (var entity in room.Entities)
            {
                //entity.AfterAction(_room);
            }
        }
    }
}