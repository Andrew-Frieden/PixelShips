using System;
using System.Collections.Generic;

namespace Models
{
    public class SimpleActionDto
    {
        public string ActionName;
        public string TargetId;
        public string SourceId;
        public Dictionary<string, int> Stats;
    }
}
