namespace Models.Factories
{
    public static class FactoryContainer
    {
        private static IRoomFactory _roomFactory;
        public static IRoomFactory RoomFactory => _roomFactory ?? (_roomFactory = new InjectedRoomFactory());
        
        private static ShipFactory _shipFactory;
        public static ShipFactory ShipFactory => _shipFactory ?? (_shipFactory = new ShipFactory());
    }
}