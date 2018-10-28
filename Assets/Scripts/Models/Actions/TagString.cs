using System.Collections.Generic;

namespace Models.Actions
{
    public class TagString
    {
        public string Text;
        public IEnumerable<UIResponseTag> Tags;

        public TagString() { Tags = new List<UIResponseTag>(); }
        public TagString(string text) { Text = text; Tags = new List<UIResponseTag>(); }
    }

    public static class TagStringExtensions
    {
        public static TagString Tag(this string text)
        {
            return new TagString(text);
        }

        public static IEnumerable<TagString> ToTagSet(this string text)
        {
            return new TagString[] { text.Tag() };
        }

        public static TagString Tag(this string text, IEnumerable<UIResponseTag> tags)
        {
            return new TagString(text) { Tags = tags };
        }

        public static IEnumerable<TagString> ToTagSet(this string text, IEnumerable<UIResponseTag> tags)
        {
            return new TagString[] { text.Tag(tags) };
        }
    }

    public enum UIResponseTag
    {
        PlayerDamaged,
        PlayerShieldsRecovered,
        PlayerEnergyConsumed,
        PlayerHullModified,
        PlayerHealed,
        ViewCmd,
        ViewBase,
        ShowHUD,
        ShowNavBar,
        HideHUD,
        HideNavBar,
        UpdateHomeworld,
        DisableCmdView
    }
}