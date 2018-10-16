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
            
            ship.SwapWeapon(roomFactory.GetRandomWeapon(Weapon.WeaponTypes.Light));
            ship.SwapWeapon(roomFactory.GetRandomWeapon(Weapon.WeaponTypes.Heavy));
            
            ship.EquipHardware(new HazardDetector());
            ship.EquipHardware(new MobDetector());
            ship.EquipHardware(new TownDetector());
            ship.EquipHardware(new GatheringBoost());
            ship.EquipHardware(new HazardMitigation());
            
            return ship;
        }
    }
}