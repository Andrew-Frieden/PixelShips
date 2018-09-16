using System;
using System.Collections.Generic;
using System.Linq;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;  // i think we should put the stat keys somewhere else - doesn't seem like roomcontroller needs to know about dtos
using Models.Stats;
using UnityEngine;
using static Models.CommandShip;

namespace Controller
{
    public static class RoomController
    {
        public delegate void OnRoomHealEvent();
        public static event OnRoomHealEvent onRoomHealEvent;
        
        public static void StartNextRoom(IRoom nextRoom, IRoom previousRoom)
        {
            nextRoom.SetPlayerShip(previousRoom.PlayerShip);
            nextRoom.PlayerShip.WarpTarget = null;
            CalculateDialogues(nextRoom);
        }

        public static IEnumerable<string> ResolveNextTick(IRoom room, IRoomAction playerAction)
        {
            var nextTickText = (List<string>) ExecuteActions(room, playerAction);
            var cleanupText = DoCleanup(room);

            //If there is any cleanup text is means the ship was destroyed, this should be explicit here
            if (cleanupText.Any())
            {
                nextTickText.AddRange(cleanupText);
            }
            //Otherwise recalculate all dialogues and do a game tick
            else
            {
                CalculateDialogues(room);
                GameManager.Instance.GameState.Tick();

                if (GameManager.Instance.GameState.GetTicks() % 5 == 0)
                {
                    nextTickText.AddRange(PassiveRoomHeal(room));
                }
            }

            return nextTickText;
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

        private static IEnumerable<string> PassiveRoomHeal(IRoom room)
        {
            //Heal the player shields for 5, max of max shields
            room.PlayerShip.Stats[StatKeys.Shields] = Mathf.Min(room.PlayerShip.Stats[StatKeys.MaxShields],
                room.PlayerShip.Stats[StatKeys.Shields] + 5);
            
            //Heal all entity shields for 3, max of max shields
            foreach (var entity in room.Entities)
            {
                if (entity.Stats.ContainsKey(StatKeys.Shields))
                {
                    entity.Stats[StatKeys.Shields] = Mathf.Min(entity.Stats[StatKeys.MaxShields],
                        entity.Stats[StatKeys.Shields] + 3);
                }
            }
            
            onRoomHealEvent?.Invoke();
            
            return new List<string>
            {
                "You march of time continues. Your shields regenerate."
            };
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
            foreach (var entity in room.Entities)
            {
                //entity.AfterAction(_room);
            }

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