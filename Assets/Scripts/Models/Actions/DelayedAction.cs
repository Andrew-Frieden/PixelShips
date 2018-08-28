﻿using System.Collections.Generic;
using Models.Actors;

namespace Models.Actions
{
    public class DelayedAction : SimpleAction
    {
        private readonly string _actorId;
        private readonly string _description;

        public DelayedAction(string description, string actorId)
        {
            _description = description;
            _actorId = actorId;
        }
        
        public override IEnumerable<string> Execute(IRoom room)
        {
            var actor = (IRoomActor) room.FindEntity(_actorId);
            actor.Stats[DelayedActor.TimeToLiveKey]--;
            return new List<string>() { _description + $"in {actor.Stats[DelayedActor.TimeToLiveKey]} ticks"};
        }
    }
}