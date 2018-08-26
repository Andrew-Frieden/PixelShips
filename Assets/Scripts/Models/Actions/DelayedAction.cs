using System.Collections.Generic;
using Models.Actors;

namespace Models.Actions
{
    public class DelayedAction : IRoomAction
    {
        private DelayedActor _actor;
        private readonly string _description;

        public DelayedAction(string description)
        {
            _description = description;
        }
        
        public IEnumerable<string> Execute(IRoom room)
        {
            return new List<string>() { _description };
        }
    }
}