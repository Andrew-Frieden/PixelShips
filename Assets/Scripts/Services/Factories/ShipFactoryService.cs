using TextSpace.Items;
using TextSpace.Models;

namespace TextSpace.Services.Factories
{
    public class ShipFactoryService
    {
        public CommandShip GenerateCommandShip(RoomFactoryService roomFactory)
        {
            var ship = new CommandShip();
            ship.SwapWeapon(roomFactory.GetRandomWeapon(Weapon.WeaponTypes.Light, 20));
            ship.SwapWeapon(roomFactory.GetRandomWeapon(Weapon.WeaponTypes.Heavy, 20));
            return ship;
        }
    }
}