using UnityEngine;
using Common;

namespace Models.Factories
{
    public static class ShipFactory
    {
        public static CommandShip GenerateCommandShip()
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