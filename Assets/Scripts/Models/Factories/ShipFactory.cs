using UnityEngine;
using Common;

namespace Models.Factories
{
    public class ShipFactory : Singleton<ShipFactory>
    {
        public CommandShip GenerateCommandShip()
        {
            return new CommandShip(
                Random.Range(0, 10),
                Random.Range(0, 10),
                Random.Range(0, 10),
                Random.Range(0, 10),
                Random.Range(0, 10),
                Random.Range(0, 10));
        }
    }
}