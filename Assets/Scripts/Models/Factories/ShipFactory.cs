using UnityEngine;
using Common;

namespace Models.Factories
{
    public class ShipFactory
    {
        public CommandShip GenerateCommandShip()
        {
            return new CommandShip();
        }
    }
}