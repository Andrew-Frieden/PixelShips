
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

        public static string GetActiveTextTag(this Ship ship, bool includeId = false)
        {
            var includedIdText = includeId ? " " + ship.Id.Substring(0,4) : string.Empty;
            var shipTag = string.Format("[{0}{1}]", ship.Name, includedIdText);
            return shipTag.AddLink(ship.Id).AddColor("purple");
        }

        public static string GetActiveTextTage(this Room room, bool includeId = false)
        {
            return string.Empty;
        }

        public static string GetActiveRoomText(this string text, string roomId)
        {
            return text.AddLink(roomId).AddColor("orange");
        }

        public static string GetActiveRoomText(string roomId)
        {
            var t = string.Format("[Sector {0}]", roomId.Substring(0, 4));
            return t.AddLink(roomId).AddColor("orange");
        }
    }
}
