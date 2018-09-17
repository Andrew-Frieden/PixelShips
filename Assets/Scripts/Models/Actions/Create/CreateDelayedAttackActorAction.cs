using System.Collections.Generic;
using Models.Actors;
using TextEncoding;

namespace Models.Actions
{
    public class CreateDelayedAttackActorAction : SimpleAction
    {
        public const string ACTION_NAME = "CreateDelayedAttackActor";
        
        private IRoomActor _source;
        private IRoomActor _target;
        private int _timeToLive;
        private int _damage;
        private string _name;

        public CreateDelayedAttackActorAction(IRoomActor source, IRoomActor target, int timeToLive, int damage, string name)
        {
            ActionName = ACTION_NAME;
            _timeToLive = timeToLive;
            _source = source;
            _target = target;
            _damage = damage;
            _name = name;
        }
        
        public override IEnumerable<StringTagContainer> Execute(IRoom room)
        {
            var newActor = new DelayedAttackActor(_source, _target, _timeToLive, _damage, _name);
            room.Entities.Add(newActor);

            if (_source is CommandShip)
            {
                return new List<StringTagContainer>()
                {
                    new StringTagContainer()
                    {
                        Text = ("You fire a < >.").Encode(_name, newActor.Id, LinkColors.HostileEntity),
                        ResultTags = new List<ActionResultTags> {ActionResultTags.Damage}
                    }
                };
            }
            
            return new List<StringTagContainer>()
            {
                new StringTagContainer()
                {
                    Text = ("You detect an incomming < >.").Encode(_name, newActor.Id, LinkColors.HostileEntity),
                    ResultTags = new List<ActionResultTags> {ActionResultTags.Damage}
                }
            };
        }
    }
}