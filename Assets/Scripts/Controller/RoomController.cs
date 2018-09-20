using System.Collections.Generic;
using System.Linq;
using Models.Actions;
using Models.Dialogue;
using Models.Stats;
using UnityEngine;

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

        public static IEnumerable<StringTagContainer> ResolveNextTick(IRoom room, IRoomAction playerAction,
            ShipHudController shipHudController, ScrollViewController scrollView)
        {
            var nextTickActionResults = (List<StringTagContainer>) ExecuteActions(room, playerAction, shipHudController, scrollView);
            nextTickActionResults.AddRange(Cleanup(room));

            CalculateDialogues(room);
            GameManager.Instance.GameState.Tick();

            return nextTickActionResults;
        }

        private static IEnumerable<StringTagContainer> ExecuteActions(IRoom room, IRoomAction playerAction, 
            ShipHudController shipHudController, ScrollViewController scrollView)
        {
            var actionResults = new List<StringTagContainer>();
            var actionsToExecute = new List<IRoomAction>()
            {
                playerAction
            };
            
            if (GameManager.Instance.GameState.GetTicks() > 0 && GameManager.Instance.GameState.GetTicks() % 5 == 0)
            {
                //  TODO: one corner case here is that you will heal enemy ships in a room even if they haven't existed for 5 ticks (if you entered the room after a few ticks passed) 
                actionsToExecute.Add(new PassiveRoomHealAction());
            }

            //TODO: Dont love that this business logic is in here 
            //The point of this is that if we are warping we don't execute anything else
            //for that tick. We could change this flow.
            if (!(playerAction is WarpAction))
            {
                foreach (var entity in room.Entities)
                {
                    actionsToExecute.Add(entity.MainAction(room));
                }
            }
            
            foreach (var action in actionsToExecute)
            {
                var results = action.Execute(room);

                foreach (var result in results)
                {
                    //  TODO this should actually happen when the scroll cell writes out the text, not when everything is executed
                    DoUIReaction(result, shipHudController, scrollView);
                    
                    actionResults.Add(result);
                }
            }

            return actionResults;
        }

        private static void DoUIReaction(StringTagContainer result, ShipHudController shipHudController, ScrollViewController scrollView)
        {
            if (result.ResultTags == null)
            {
                return;
            }
            
            if (result.ResultTags.Contains(ActionResultTags.Damage))
            {
                shipHudController.UpdateShield();
                shipHudController.UpdateHull();
                scrollView.Shake();
            }
                    
            if (result.ResultTags.Contains(ActionResultTags.Heal))
            {
                shipHudController.UpdateShield();
            }
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

        private static IEnumerable<StringTagContainer> Cleanup(IRoom room)
        {
            room.Entities.ForEach(e => e.CleanupStep(room));

            var destroyed = room.Entities.Where(e => e.IsDestroyed).ToList();
            destroyed.ForEach(e => room.Entities.Remove(e));

            if (room.PlayerShip.IsDestroyed)
            {
                return new List<StringTagContainer>()
                {
                    new StringTagContainer()
                    {
                        Text = "You have been destroyed.",
                        ResultTags = new List<ActionResultTags> { }
                    },
                    new StringTagContainer()
                    {
                        Text = "Navigate to your base to recruit a new captain.",
                        ResultTags = new List<ActionResultTags> { }
                    }
                };
            }

            return new List<StringTagContainer>();
        }
    }
}