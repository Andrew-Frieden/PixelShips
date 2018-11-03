using System.Linq;
using TextEncoding;
using TextSpace.Framework;
using TextSpace.Items;
using TextSpace.Models;
using TextSpace.Models.Actions;
using TextSpace.Models.Dialogue;

namespace TextSpace.Services
{
    public class NavigationService : IResolvableService
    {
        private IShipProvider ShipProvider;
        private IRoomProvider RoomProvider;

        private Room Room => RoomProvider.Room;
        private CommandShip PlayerShip => ShipProvider.Ship;

        public NavigationService(IShipProvider shipProvider, IRoomProvider roomProvider)
        {
            ShipProvider = shipProvider;
            RoomProvider = roomProvider;
        }
    
        public ABDialogueContent NavigationDialogue()
        {
            if (Room.PlayerShip.WarpDriveReady)
            {
                var templateA = Room.Exits.First();
                var templateB = Room.Exits.Last();
                var aText = GetRoomExitText(templateA);
                var bText = GetRoomExitText(templateB);

                return DialogueBuilder.Init()
                    .AddMainText($"{Room.Description}{Env.ll}Your warp drive is fully charged.")
                    .AddOption(aText, new WarpAction(templateA))
                    .AddOption(bText, new WarpAction(templateB))
                    .Build(Room);
            }

            return DialogueBuilder.Init()
                .AddMainText($"{Room.Description}{Env.ll}Your warp drive is cold.")
                .AddTextA("Spin up warp drive.")
                .AddActionA(new WarpDriveReadyAction(Room.PlayerShip))
                .Build(Room);
        }

        private string GetRoomExitText(RoomTemplate t)
        {
            var text = $"Warp to {Room.GetNameForFlavor(t.Flavor)}{Env.ll}";

            if (t.ActorFlavors.Contains(RoomActorFlavor.Hazard) 
                && (PlayerShip.CheckHardware<HazardDetector>() || PlayerShip.CheckHardware<SuperDetector>()))
            {
                text += "Hazard Detected" + Env.l;
            }

            if (t.ActorFlavors.Contains(RoomActorFlavor.Mob)
                && (PlayerShip.CheckHardware<MobDetector>() || PlayerShip.CheckHardware<SuperDetector>()))
            {
                text += "Hostile Detected" + Env.l;
            }

            if (t.ActorFlavors.Contains(RoomActorFlavor.Town)
                && (PlayerShip.CheckHardware<TownDetector>() || PlayerShip.CheckHardware<SuperDetector>()))
            {
                text += "Starport Detected" + Env.l;
            }

            return text;
        }
    }
}
