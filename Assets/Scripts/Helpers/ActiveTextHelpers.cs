
using PixelSpace.Models.SharedModels;
using PixelSpace.Models.SharedModels.Ships;

namespace PixelShips.Helpers
{
    public static class ActiveTextHelpers
    {
        public static string AddLink(this string text, string id)
        {
            return string.Format("<link={0}>{1}</link>", id, text);
        }

        public static string AddColor(this string text, string color)
        {
            return string.Format("<color={0}>{1}</color>", color, text);
        }

        public static string GetActive(this string text, Ship ship)
        {
            return text.AddLink(ship.Id).AddColor("purple");
        }

        public static string GetActiveRoomText(this string text, string roomId)
        {
            return text.AddLink(roomId).AddColor("orange");
        }
    }
}
