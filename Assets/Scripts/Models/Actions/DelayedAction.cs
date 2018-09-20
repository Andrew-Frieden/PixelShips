using System.Collections.Generic;
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
        
        public DelayedAction(string actorId)
        {
            _actorId = actorId;
        }
        
        public DelayedAction()
        {
            
        }
        
        public override IEnumerable<TagString> Execute(IRoom room)
        {
            var actor = (IRoomActor) room.FindEntity(_actorId);

            if (actor != null)
            {
                actor.Stats[TemporaryEntity.TimeToLiveKey]--;
            }

            if (_description != null)
            {
                return new List<TagString>()
                {
                    new TagString()
                    {
                        Text = _description + $"in {actor.Stats[TemporaryEntity.TimeToLiveKey]} ticks." ,
                        Tags = new List<EventTag> { }
                    }
                };
            }
            
            return new List<TagString>() { };
        }
    }
}