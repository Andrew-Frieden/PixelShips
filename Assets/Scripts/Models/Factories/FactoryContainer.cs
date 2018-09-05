namespace Models.Factories
{
    public static class FactoryContainer
    {
        private static RoomFactory _roomFactory;
        public static RoomFactory RoomFactory => _roomFactory ?? (_roomFactory = new RoomFactory());
        
        private static ShipFactory _shipFactory;
        public static ShipFactory ShipFactory => _shipFactory ?? (_shipFactory = new ShipFactory());
    }
}