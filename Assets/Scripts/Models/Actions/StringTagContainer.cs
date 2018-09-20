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

    public enum EventTag
    {
        Damage,
        Heal
    }
}