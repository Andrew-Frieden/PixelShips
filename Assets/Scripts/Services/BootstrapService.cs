using TextSpace.Framework;
using TextSpace.Models;
using TextSpace.Services.Factories;

namespace TextSpace.Services
{
    public class BootstrapService : IResolvableService
    {
        public BootstrapService(ExpeditionFactoryService expService)
        {
            DevGameState = new GameState
            {
                Expedition = expService.CreateBootstrapExpedition(FTUE: false),
                Home = new Homeworld() { PlanetName = "Earth", Description = "Developer" }
            };

            FTUEGameState = new GameState
            {
                Expedition = expService.CreateBootstrapExpedition(),
                Home = new Homeworld() { PlanetName = "???", Description = "???" }
            };
        }

        public GameState DevGameState { get; private set; }
        public GameState FTUEGameState { get; private set; }
    }
}
