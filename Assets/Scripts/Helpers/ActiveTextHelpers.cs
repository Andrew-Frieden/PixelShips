
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
    }
}
