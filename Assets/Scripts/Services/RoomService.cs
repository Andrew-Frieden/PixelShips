using System.Collections.Generic;
using System.Linq;
using TextSpace.Models.Actions;
using EnumerableExtensions;

namespace TextSpace.Services
{
    public static class RoomService
    {
        public static void StartNextRoom(IRoom nextRoom, IRoom previousRoom)
        {
            nextRoom.SetPlayerShip(previousRoom.PlayerShip);
            nextRoom.PlayerShip.WarpTarget = null;
            CalculateDialogues(nextRoom);
        }

        public static IEnumerable<TagString> ResolveNextTick(IRoom room, IRoomAction playerAction)
        {
            var resolveResults = new List<TagString>();
            resolveResults.AddRange(ExecuteMainActions(room, playerAction));
            resolveResults.AddRange(ExecuteCleanupActions(room));

            // do mission stuff

            CalculateDialogues(room);
            GameManager.Instance.GameState.Tick();

            return resolveResults;
        }

        private static IEnumerable<TagString> ExecuteMainActions(IRoom room, IRoomAction playerAction)
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
            //  calculate dialogues for every entity in the room
            room.Entities.ForEach(n => n.CalculateDialogue(room));
            
            //  calculate dialogues for the player ship and all sub-entities on it
            room.PlayerShip.CalculateDialogue(room);
            room.PlayerShip.Hardware.ForEach(h => h.CalculateDialogue(room));
            room.PlayerShip.LightWeapon.CalculateDialogue(room);
            room.PlayerShip.HeavyWeapon.CalculateDialogue(room);

            //  calculate dialogue for the room itself
            //  include mission stuff when calculating room navigation options
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
                        Tags = new List<UIResponseTag> { }
                    },
                    new TagString()
                    {
                        Text = "Navigate to your base to recruit a new captain.",
                        Tags = new List<UIResponseTag> { }
                    }
                });
            }
            return results;
        }
    }
}