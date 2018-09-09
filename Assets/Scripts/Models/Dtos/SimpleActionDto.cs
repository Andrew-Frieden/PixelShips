using System.Collections.Generic;

namespace Models.Dtos
{
    public class SimpleActionDto
    {
        public string ActionName;
        public string TargetId;
        public string SourceId;
        public Dictionary<string, int> Stats;
        public Dictionary<string, string> Values;
    }
}
