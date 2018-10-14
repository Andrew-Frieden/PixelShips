using UnityEngine;
using Common;
using Items;

namespace Models.Factories
{
    public class ShipFactory
    {
        public CommandShip GenerateCommandShip(RoomFactory roomFactory)
        {
            return new CommandShip(roomFactory.GetRandomWeapon(Weapon.WeaponTypes.Light), 
                                   roomFactory.GetRandomWeapon(Weapon.WeaponTypes.Heavy));
        }
    }
}