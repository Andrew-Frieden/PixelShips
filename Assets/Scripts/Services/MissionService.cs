using System;
using TextSpace.Framework;
using TextSpace.Models;

namespace TextSpace.Services
{
    public class MissionService : IResolvableService
    {
        private readonly IMissionProvider _missionProvider;

        public MissionService(IMissionProvider missionProvider)
        {
            _missionProvider = missionProvider;
        }

        public bool HasActiveMission => _missionProvider.Mission != null;

        private Mission CreateMission()
        {
            throw new NotImplementedException();
        }

        public IRoomActor SpawnMissionActor()
        {
            throw new NotImplementedException();
        }

        public bool ShouldSpawnMissionActor()
        {
            throw new NotImplementedException();
        }
    }
}