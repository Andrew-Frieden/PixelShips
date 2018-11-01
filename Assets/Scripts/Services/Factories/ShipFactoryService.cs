using TextSpace.Framework;
using TextSpace.Items;
using TextSpace.Models;

namespace TextSpace.Services.Factories
{
    public class ShipFactoryService : IResolvableService
    {
        private readonly RoomFactoryService _roomFactory;

        public ShipFactoryService(RoomFactoryService roomFactory)
        {
            _roomFactory = roomFactory;
        }

        public CommandShip GenerateCommandShip()
        {
            var ship = new CommandShip();
            ship.SwapWeapon(_roomFactory.GetRandomWeapon(Weapon.WeaponTypes.Light, 20));
            ship.SwapWeapon(_roomFactory.GetRandomWeapon(Weapon.WeaponTypes.Heavy, 20));
            return ship;
        }
    }
}