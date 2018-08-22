using System;
using System.Collections.Generic;

namespace Models
{
    public class SimpleActionDto
    {
        public string ActionName;
        public string TargetId;
        public string SourceId;
        public List<string> Keys;
        public List<int> Values;
    }
}
