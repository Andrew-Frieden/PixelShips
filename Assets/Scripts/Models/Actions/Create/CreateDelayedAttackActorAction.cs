using System.Collections.Generic;
using Models.Actors;
using Models.Stats;
using TextEncoding;

namespace Models.Actions
{
    public class CreateDelayedAttackActorAction : SimpleAction
    {
        public const string ACTION_NAME = "CreateDelayedAttackActor";
        
        private int _timeToLive;
        private int _damage;
        private string _name;

        public CreateDelayedAttackActorAction(IRoomActor source, IRoomActor target, int timeToLive, int damage,
            string name, int energy)
        {
            ActionName = ACTION_NAME;
            
            _timeToLive = timeToLive;
            _damage = damage;
            _name = name;

            Source = source;
            Target = target;
            Energy = energy;
        }
        
        public override IEnumerable<TagString> Execute(IRoom room)
        {
            base.Execute(room);
            
            var newActor = new DelayedAttackActor(Source, Target, _timeToLive, _damage, _name);
            room.Entities.Add(newActor);

            if (Source is CommandShip)
            {
                return new List<TagString>()
                {
                    new TagString()
                    {
                        Text = ("You fire a < >.").Encode(_name, newActor.Id, LinkColors.HostileEntity),
                        Tags = ActionTags
                    }
                };
            }
            
            return new List<TagString>()
            {
                new TagString()
                {
                    Text = ("You detect an incomming < >.").Encode(_name, newActor.Id, LinkColors.HostileEntity)
                }
            };
        }
    }
}