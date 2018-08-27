using System.Collections.Generic;
using Models.Actors;

namespace Models.Actions
{
    public class DelayedAction : SimpleAction
    {
        private readonly DelayedActor _actor;
        private readonly string _description;

        public DelayedAction(string description, DelayedActor actor)
        {
            _description = description;
            _actor = actor;
        }
        
        public override IEnumerable<string> Execute(IRoom room)
        {
            _actor.Stats[DelayedActor.TimeToLiveKey]--;
            return new List<string>() { _description + $"in {_actor.Stats[DelayedActor.TimeToLiveKey]} ticks"};
        }
    }
}