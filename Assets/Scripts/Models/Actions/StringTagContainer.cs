using System.Collections.Generic;

namespace Models.Actions
{
    public class StringTagContainer
    {
        public string Text;
        public IEnumerable<ActionResultTags> ResultTags;

        public StringTagContainer() { }
        public StringTagContainer(string text) { Text = text; }
    }

    public enum ActionResultTags
    {
        Damage,
        Heal
    }
}