using System.Collections.Generic;
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
        private MissionService MissionService;

        private Room Room => RoomProvider.Room;
        private CommandShip PlayerShip => ShipProvider.Ship;

        public NavigationService(IShipProvider shipProvider, IRoomProvider roomProvider, MissionService missionService)
        {
            ShipProvider = shipProvider;
            RoomProvider = roomProvider;
            MissionService = missionService;
        }

        public void OnPlayerNavigate(IRoom room, RoomTemplate template, Mission missionSelected)
        {
            var missionStatus = MissionService.GetStatus();
            switch (missionStatus)
            {
                case MissionStatus.NeedsToSelectMission:
                    updates.Add("Mission Accepted");
                    MissionService.StartMission(missionSelected);
                    break;
                case MissionStatus.InMissionRoom:
                    updates.Add("Mission Failed");
                    MissionService.FailMission();
                    break;
                case MissionStatus.LookingForMissionRoom:
                    MissionService.AdvanceMissionProgress(room, template);
                    break;
                default:
                    break;
            }
        }

        private List<string> updates = new List<string>();
        public IEnumerable<string> GetNavigationUpdates()
        {
            var text = new List<string>(updates);
            updates.Clear();
            return text;
        }
    
        public ABDialogueContent NavigationDialogue()
        {
            if (Room.PlayerShip.WarpDriveReady)
            {
                if (MissionService.HasActiveMission)
                {
                    var templateA = Room.Exits.First();
                    var templateB = Room.Exits.Last();
                    var aText = GetRoomExitText(templateA);
                    var bText = GetRoomExitText(templateB);

                    //  if its the mission room we need to tell the user they will fail the mission if they warp out

                    return DialogueBuilder.Init()
                        .AddMainText($"{Room.Description}{Env.ll}Your warp drive is fully charged.")
                        .AddOption(aText, new WarpAction(templateA))
                        .AddOption(bText, new WarpAction(templateB))
                        .Build(Room);
                }
                else
                {
                    var templateA = Room.Exits.First();
                    var templateB = Room.Exits.Last();

                    var newMissionA = MissionService.CreateMission(templateA);
                    var newMissionB = MissionService.CreateMission(templateB);

                    var aText = GetRoomExitText(templateA, newMissionA);
                    var bText = GetRoomExitText(templateB, newMissionB);

                    return DialogueBuilder.Init()
                        .AddMainText($"{Env.l}Your warp drive finishes spinning up as you recieve encrypted subspace comms from your homeworld.{Env.ll}" +
                                $"Looks like mission data with advanced jump codes")
                        .AddOption(aText, new WarpAction(templateA, newMissionA))
                        .AddOption(bText, new WarpAction(templateB, newMissionB))
                        .Build(Room);
                }
            }

            return DialogueBuilder.Init()
                .AddMainText($"{Room.Description}{Env.ll}Your warp drive is cold.")
                .AddTextA("Spin up warp drive.")
                .AddActionA(new WarpDriveReadyAction(Room.PlayerShip))
                .Build(Room);
        }

        private string GetRoomExitText(RoomTemplate t, Mission mission)
        {
            var text = $"Select Mission{Env.ll}" +
                $"Jump Location: {Room.GetNameForFlavor(t.Flavor)}{Env.ll}" +
                $"Objective:{Env.l}" +
                $"Destroy the {mission.Objective.GetLinkText()}{Env.l}" +
                $"somewhere in {Room.GetNameForFlavor(mission.RoomFlavor)}";
            return text;
        }

        private string GetRoomExitText(RoomTemplate t)
        {
            var text = $"Warp to {Room.GetNameForFlavor(t.Flavor)}{Env.ll}";
            text += GetDetectionText(t);
            return text;
        }

        private string GetDetectionText(RoomTemplate t)
        {
            var text = string.Empty;

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

            if (t.IsMission)
            {
                text += "Objective Detected" + Env.l;
            }

            return text;
        }
    }
}
