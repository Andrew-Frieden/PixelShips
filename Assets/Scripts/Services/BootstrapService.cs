using TextSpace.Framework;
using TextSpace.Models;
using TextSpace.Services.Factories;

namespace TextSpace.Services
{
    public class BootstrapService : IResolvableService
    {
        public BootstrapService(ShipFactoryService shipFactory, RoomFactoryService roomFactory)
        {
            DevGameState = new GameState
            {
                CurrentExpedition = new Expedition
                {
                    CmdShip = shipFactory.GenerateCommandShip(),
                    Room = (Room)roomFactory.GenerateBootstrapRoom(false)
                },
                Home = new Homeworld() { PlanetName = "???", Description = "???" }
            };

            GameState = new GameState
            {
                CurrentExpedition = new Expedition
                {
                    CmdShip = shipFactory.GenerateCommandShip(),
                    Room = (Room)roomFactory.GenerateBootstrapRoom(true)
                },
                Home = new Homeworld() { PlanetName = "???", Description = "???" }
            };
        }

        public GameState DevGameState { get; private set; }
        public GameState GameState { get; private set; }
    }
}
