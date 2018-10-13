﻿using System.Collections.Generic;
using System.Linq;
using Models.Actions;
using Models.Dialogue;
using Models.Stats;
using UnityEngine;
using EnumerableExtensions;

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

        public static IEnumerable<TagString> ResolveNextTick(IRoom room, IRoomAction playerAction,
            ShipHudController shipHudController, ScrollViewController scrollView)
        {
            var resolveResults = new List<TagString>();
            resolveResults.AddRange(ExecuteMainActions(room, playerAction, shipHudController, scrollView));
            resolveResults.AddRange(ExecuteCleanupActions(room));

            CalculateDialogues(room);
            GameManager.Instance.GameState.Tick();

            return resolveResults;
        }

        private static IEnumerable<TagString> ExecuteMainActions(IRoom room, IRoomAction playerAction, 
            ShipHudController shipHudController, ScrollViewController scrollView)
        {
            var actionResults = new List<TagString>();
            var actionsToExecute = new List<IRoomAction>()
            {
                playerAction
            };
            
            if (GameManager.Instance.GameState.GetTicks() > 0 && GameManager.Instance.GameState.GetTicks() % 5 == 0 && !(playerAction is WarpAction))
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
                    actionResults.Add(result);
                }
            }

            return actionResults;
        }

        private static void CalculateDialogues(IRoom room)
        {
            room.Entities.ForEach(n => n.CalculateDialogue(room));
            
            room.PlayerShip.CalculateDialogue(room);
            room.PlayerShip.Hardware.ForEach(h => h.CalculateDialogue(room));

            room.CalculateDialogue();
        }

        private static IEnumerable<TagString> ExecuteCleanupActions(IRoom room)
        {
            //  run all cleanup steps and collect any cleanup actions to execute
            var actions = new List<IRoomAction>()
            {
                room.PlayerShip.CleanupStep(room)
            };
            room.Entities.ForEach(n => actions.Add(n.CleanupStep(room)));

            //  execute all the cleanup actions
            var results = new List<TagString>();
         
            foreach (var a in actions)
            {
                var r = a.Execute(room);
                results.AddRange(r);
            }

            //  remove any destroyed entities from the room
            var destroyed = room.Entities.Where(e => e.IsDestroyed).ToList();
            destroyed.ForEach(e => room.Entities.Remove(e));

            if (room.PlayerShip.IsDestroyed)
            {
                results.AddRange(new TagString[]
                {
                    new TagString()
                    {
                        Text = "You have been destroyed.",
                        Tags = new List<EventTag> { }
                    },
                    new TagString()
                    {
                        Text = "Navigate to your base to recruit a new captain.",
                        Tags = new List<EventTag> { }
                    }
                });
            }
            return results;
        }
    }
}