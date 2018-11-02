using TextSpace.Framework;
using TextSpace.Items;
using TextSpace.Models;

namespace TextSpace.Services.Factories
{
    public class ShipFactoryService : IResolvableService
    {
        private readonly WeaponFactoryService _weaponFactory;

        public ShipFactoryService(WeaponFactoryService weaponFactory)
        {
            _weaponFactory = weaponFactory;
        }

        public CommandShip GenerateCommandShip()
        {
            var ship = new CommandShip();
            ship.SwapWeapon(_weaponFactory.GetRandomWeapon(Weapon.WeaponTypes.Light, 20));
            ship.SwapWeapon(_weaponFactory.GetRandomWeapon(Weapon.WeaponTypes.Heavy, 20));
            return ship;
        }
    }
}