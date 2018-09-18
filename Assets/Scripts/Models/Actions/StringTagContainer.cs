using System.Collections.Generic;

namespace Models.Actions
{
    public class StringTagContainer
    {
        public string Text;
        public IEnumerable<ActionResultTags> ResultTags;
    }

    public enum ActionResultTags
    {
        Damage,
        Heal
    }
}