using System.Collections.Generic;
using Models.Actors;
using TextEncoding;

namespace Models.Actions
{
    public class CreateShieldActorAction : SimpleAction
    {
        public const string ACTION_NAME = "CreateShieldActor";
        
        private int _timeToLive;
        private int _damageReduction;

        public CreateShieldActorAction(IRoomActor source, IRoomActor target, int timeToLive, int damageReduction)
        {
            ActionName = ACTION_NAME;
            _timeToLive = timeToLive;
            Source = source;
            Target = target;
            _damageReduction = damageReduction; 
        }
        
        public override IEnumerable<string> Execute(IRoom room)
        {
            room.Entities.Add(new ShieldActor(Source, Target, _timeToLive, _damageReduction));
            
            return new List<string>() { "<> generate a shield.".Encode("You", Source.Id, "blue") };
        }
    }
}