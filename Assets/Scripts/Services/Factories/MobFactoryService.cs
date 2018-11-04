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

        public IEnumerable<IRoomActor> BuildMob(RoomTemplate template)
        {
            var actors = new List<IRoomActor>();

            var mob = MobData
                .Where(d => d.RoomFlavors.Contains(template.Flavor))
                .OrderBy(d => Math.Abs(template.PowerLevel - d.Powerlevel))
                .First().FromFlexData();

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

            var loot = GatherableData.Where(h => h.RoomFlavors.Contains(template.Flavor) 
                            && h.Powerlevel <= template.PowerLevel).GetRandom().FromFlexData();
            loot.IsHidden = true;
            loot.DependentActorId = mob.Id;
            actors.Add(loot);

            actors.Add(mob);
            return actors;
        }
    }
}
