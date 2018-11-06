
using TextSpace.Models;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using TextSpace.Framework.IoC;
using TextSpace.Services;

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

        public static string Encode(this string text, ITextEntity entity, string color)
        {
            return Encode(text, entity.GetLinkText(), entity.Id, color);
        }

        public static string Encode(this string text, string link, string id, string color)
        {
            var missionService = ServiceContainer.Resolve<MissionService>();
            var isObjective = missionService.IsMissionObjective(id);
            var linkText = isObjective ? $"[*{link}*]" : $"[{link}]";
            return Regex.Replace(text, "<.*?>", GetLink(linkText, id, color));
        }
    }

    public static class Env
    {
        public static string l = Environment.NewLine;
        public static string ll = Environment.NewLine + Environment.NewLine;
    }
}
