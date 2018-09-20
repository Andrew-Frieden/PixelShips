using System.Collections.Generic;

namespace Models.Actions
{
    public class TagString
    {
        public string Text;
        public IEnumerable<EventTag> Tags;

        public TagString() { }
        public TagString(string text) { Text = text; }
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
    }

    public enum EventTag
    {
        Damage,
        Heal
    }
}