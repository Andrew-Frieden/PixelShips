using System;
using System.Collections.Generic;

namespace Models.Actions
{
    public abstract class SimpleAction : IRoomAction
    {
        public string ActionName;

        public ITextEntity Target;
        public ITextEntity Source;
        public Dictionary<string, int> Stats;

        public abstract IEnumerable<string> Execute(IRoom room);
    }
}
