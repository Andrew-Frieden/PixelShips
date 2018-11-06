using System;
using System.Collections.Generic;
using System.Linq;
using TextSpace.Framework;
using TextSpace.Models;
using TextSpace.Models.RoomEntities.Mobs;
using TextSpace.Services.Factories;

namespace TextSpace.Services
{
    public enum MissionStatus
    {
        NeedsToSelectMission = 0,
        InMissionRoom = 1,
        LookingForMissionRoom = 2
    }

    public class MissionService : IResolvableService
    {
        private readonly IMissionProvider _missionProvider;
        private readonly IRoomProvider _roomProvider;
        private readonly MobFactoryService mobFactory;

        public MissionService(IMissionProvider missionProvider, MobFactoryService mobFactorySvc, IRoomProvider roomProvider)
        {
            _missionProvider = missionProvider;
            _roomProvider = roomProvider;
            mobFactory = mobFactorySvc;
        }

        public bool HasActiveMission => _missionProvider.Mission != null;
        private Mission CurrentMission => _missionProvider.Mission;

        public MissionStatus GetStatus()
        {
            if (CurrentMission == null)
                return MissionStatus.NeedsToSelectMission;

            if (_roomProvider.Room.FindEntity(CurrentMission.Objective.Id) != null)
                return MissionStatus.InMissionRoom;

            return MissionStatus.LookingForMissionRoom;
        }

        public bool IsMissionObjective(string id)
        {
            if (GetStatus() == MissionStatus.InMissionRoom)
            {
                return CurrentMission.Objective.Id == id;
            }
            return false;
        }

        public void StartMission(Mission mission)
        {
            _missionProvider.Mission = mission;
        }

        public void FailMission()
        {
            _missionProvider.Mission = null;
        }

        public Mission CreateMission(RoomTemplate template)
        {
            var roomFlavors = Enum.GetValues(typeof(RoomFlavor));
            var nextRoomFlavor = (RoomFlavor)roomFlavors.GetValue(UnityEngine.Random.Range(0, roomFlavors.Length));

            var mission = new Mission
            {
                MissionLevel = template.PowerLevel,
                MissionType = MissionType.DestroyMob,
                RoomFlavor = nextRoomFlavor
            };

            mission.Entities = mobFactory.BuildMob(nextRoomFlavor, template.PowerLevel * 2);
            mission.Objective = mission.Entities.Single(n => n is Mob);

            return mission;
        }

        public IEnumerable<IRoomActor> GetMissionActors()
        {
            return CurrentMission.Entities;
        }

        public void AdvanceMissionProgress(IRoom room, RoomTemplate nextRoom)
        {
            if (CurrentMission.RoomFlavor == room.Flavor 
                && CurrentMission.RoomFlavor == nextRoom.Flavor)
            {
                CurrentMission.ZoneJumps++;
            }
            else
            {
                CurrentMission.ZoneJumps = 0;
            }
        }

        public bool ShouldFindMissionExit(RoomTemplate template)
        {
            if (HasActiveMission)
            {
                if (CurrentMission.ZoneJumps >= 1 && CurrentMission.RoomFlavor == template.Flavor)
                {
                    return true;
                }
            }
            return false;
        }
    }
}