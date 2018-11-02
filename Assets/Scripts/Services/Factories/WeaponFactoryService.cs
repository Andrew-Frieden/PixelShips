using System.Collections.Generic;
using System.Linq;
using EnumerableExtensions;
using TextSpace.Framework;
using TextSpace.Items;
using TextSpace.Models;
using TextSpace.Models.Stats;
using TextSpace.RoomEntities;

namespace TextSpace.Services.Factories
{
    public class WeaponFactoryService : IResolvableService
    {
        private readonly ContentLoadService contentLoadService;
        private IEnumerable<FlexData> WeaponData => contentLoadService.Content.Weapons;
    
        public WeaponFactoryService(ContentLoadService contentSvc)
        {
            contentLoadService = contentSvc;
        }
        
        public Weapon GetRandomWeapon(Weapon.WeaponTypes type, int powerLevel)
        {
            return (Weapon) WeaponData
                .Where(w => w.Stats[StatKeys.WeaponType] == (int) type 
                    && w.Powerlevel <= powerLevel).GetRandom().FromFlexData();
        }
        
        public Weapon GetWeapon(string weaponId)
        {
            return (Weapon)WeaponData.First(wpn => wpn.Values[ValueKeys.WeaponId] == weaponId).FromFlexData();
        }
        
        public IEnumerable<IRoomActor> GetWeaponDependents(IHaveDependents parent)
        {
            var weapons = new List<IRoomActor>();
            var dependentData = parent.FindWeaponDependents(WeaponData);
            foreach (var d in dependentData)
            {
                var dependent = d.FromFlexData();
                dependent.IsHidden = true;
                dependent.DependentActorId = parent.Id;
                weapons.Add(dependent);
            }
            return weapons;
        }
    }
}
