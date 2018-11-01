using TextSpace.Framework;
using TextSpace.Models;

namespace TextSpace.Services.Factories
{
    public class ExpeditionFactoryService : IResolvableService
    {
        private ShipFactoryService _shipFactory;
        private RoomFactoryService _roomFactory;
        private IHomeworldProvider _homeProvider;

        public ExpeditionFactoryService(ShipFactoryService shipFactory, RoomFactoryService roomFactory, IHomeworldProvider homeProvider)
        {
            _shipFactory = shipFactory;
            _roomFactory = roomFactory;
            _homeProvider = homeProvider;
        }

        public Expedition CreateBootstrapExpedition(bool FTUE = true)
        {
            return new Expedition(_shipFactory.GenerateCommandShip(), _roomFactory.GenerateBootstrapRoom(FTUE));
        }

        public Expedition CreateNewExpedition()
        {
            return new Expedition(_shipFactory.GenerateCommandShip(), _roomFactory.GenerateHomeworldRoom(_homeProvider.Home));
        }
    }
}