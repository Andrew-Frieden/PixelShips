
using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TextEncoding
{
    public static class ActiveTextHelpers
    {
        public static string GetLink(this string text, string id, string color)
        {
            return string.Format("<link=\"{0}\">" + text.GetColor(color) + "</link>", id);
        }

        private static string GetColor(this string text, string color)
        {
            return string.Format("<color={0}>{1}</color>", color, text);
        }

        public static string Encode(this string text, string link, string id, string color)
        {
            return Regex.Replace(text, "<.*?>", GetLink($"[{link}]", id, color));
        }
    }

    public static class Env
    {
        public static string l = Environment.NewLine;
        public static string ll = Environment.NewLine + Environment.NewLine;
    }
}
