using EnumerableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TextSpace.Framework;
using TextSpace.Models;
using TextSpace.Models.Stats;

namespace TextSpace.Services.Factories
{
    public class MobFactoryService : IResolvableService
    {
        private readonly ContentLoadService contentLoadService;
        private readonly WeaponFactoryService weaponFactoryService;
        private IEnumerable<FlexData> MobData => contentLoadService.Content.Mobs;
        private IEnumerable<FlexData> GatherableData => contentLoadService.Content.Gatherables;

        public MobFactoryService(ContentLoadService contentSvc, WeaponFactoryService weaponFactory)
        {
            contentLoadService = contentSvc;
            weaponFactoryService = weaponFactory;
        }

        public IEnumerable<IRoomActor> BuildMob(RoomFlavor flavor, int powerLevel)
        {
            var actors = new List<IRoomActor>();

            var mob = MobData
                .Where(d => d.RoomFlavors.Contains(flavor))
                .OrderBy(d => Math.Abs(powerLevel - d.Powerlevel))
                .First().FromFlexData();

            actors.Add(mob);

            foreach (var weaponKey in ValueKeys.WeaponIds)
            {
                if (mob.Values.ContainsKey(weaponKey))
                {
                    var weapon = weaponFactoryService.GetWeapon(mob.Values[weaponKey]);
                    weapon.IsHidden = true;
                    weapon.DependentActorId = mob.Id;
                    actors.Add(weapon);
                }
            }

            var loot = GatherableData.Where(h => h.RoomFlavors.Contains(flavor) 
                            && h.Powerlevel <= powerLevel).GetRandom().FromFlexData();
            loot.IsHidden = true;
            loot.DependentActorId = mob.Id;
            actors.Add(loot);

            return actors;
        }
    }
}
