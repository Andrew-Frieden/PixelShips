using System;
using System.Collections.Generic;

namespace Models
{
    public class SimpleAction : IRoomAction
    {
        public ITextEntity Target;
        public ITextEntity Source;
        public IEnumerable<string> Keys;
        public IEnumerable<int> Values;

        public IEnumerable<string> Execute(IRoom room)
        {
            throw new NotImplementedException();
        }
    }
}
