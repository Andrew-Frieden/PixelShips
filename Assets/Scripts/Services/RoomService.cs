using System.Collections.Generic;
using System.Linq;
using TextSpace.Models.Actions;
using EnumerableExtensions;
using TextSpace.Models;
using TextSpace.Framework;
using TextSpace.Services.Factories;

namespace TextSpace.Services
{
    public class RoomService : IResolvableService
    {
        private readonly RoomFactoryService RoomFactory;
        private readonly IShipProvider ShipProvider;
        private readonly IRoomProvider RoomProvider;

        private Room Room => RoomProvider.Room;
        private CommandShip PlayerShip => ShipProvider.Ship;

        public RoomService(RoomFactoryService roomFactory, IShipProvider shipProvider, IRoomProvider roomProvider)
        {
            RoomFactory = roomFactory;
            ShipProvider = shipProvider;
            RoomProvider = roomProvider;
        }

        public void StartRoom()
        {
            var warpTarget = PlayerShip.WarpTarget;
            if (warpTarget != null)
                RoomProvider.Room = (Room)RoomFactory.GenerateRoom(warpTarget);
            PlayerShip.WarpTarget = null;
            CalculateDialogues(Room);
        }

        public IEnumerable<TagString> ResolveNextTick(IRoom room, IRoomAction playerAction)
        {
            var resolveResults = new List<TagString>();
            resolveResults.AddRange(ExecuteMainActions(room, playerAction));
            resolveResults.AddRange(ExecuteCleanupActions(room));

            CalculateDialogues(room);
            GameManager.Instance.GameState.Tick();

            return resolveResults;
        }

        private IEnumerable<TagString> ExecuteMainActions(IRoom room, IRoomAction playerAction)
        {
            var actionResults = new List<TagString>();
            var actionsToExecute = new List<IRoomAction>()
            {
                playerAction
            };
            
            if (GameManager.Instance.GameState.Ticks > 0 && GameManager.Instance.GameState.Ticks % 5 == 0 && !(playerAction is WarpAction))
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

        private void CalculateDialogues(IRoom room)
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

        private IEnumerable<TagString> ExecuteCleanupActions(IRoom room)
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