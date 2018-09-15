namespace Models.Dtos
{
    public class ShipDto
    {
        public string Id;
        
        public int Hull;
        public int Gathering;
        public int Transport;
        public int Intelligence;
        public int Combat;
        public int Speed;

        public ABContentDto ContentDto;
        
        public static class StatKeys
        {
            public const string IsAlive = "is_alive";
            public const string WarpDriveReady = "warp_drive_ready";
            public const string CaptainName = "captain_name";
        }
    }
}
