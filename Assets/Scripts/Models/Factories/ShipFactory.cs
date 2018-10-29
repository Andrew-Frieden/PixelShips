using UnityEngine;
using Common;
using Items;

namespace Models.Factories
{
    public class ShipFactory
    {
        public CommandShip GenerateCommandShip(RoomFactory roomFactory)
        {
            var ship = new CommandShip();
            ship.SwapWeapon(roomFactory.GetRandomWeapon(Weapon.WeaponTypes.Light, 20));
            ship.SwapWeapon(roomFactory.GetRandomWeapon(Weapon.WeaponTypes.Heavy, 20));
            return ship;
        }
    }
}