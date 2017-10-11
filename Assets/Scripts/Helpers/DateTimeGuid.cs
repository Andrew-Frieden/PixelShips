using System;

namespace PixelShips.Helpers
{
    public class DateTimeGuid
    {
        public DateTime Time { get; set; }
        public string Id { get; set; }

        public DateTimeGuid(string id, DateTime timeStamp)
        {
            Id = id;
            Time = timeStamp;
        }
    }
}
