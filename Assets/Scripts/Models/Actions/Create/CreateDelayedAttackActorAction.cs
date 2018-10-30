using System.Collections.Generic;
using TextSpace.Models.Actors;
using TextSpace.Models.Dtos;
using TextEncoding;

namespace TextSpace.Models.Actions
{
    public class CreateDelayedAttackActorAction : SimpleAction
    {
        public CreateDelayedAttackActorAction(IRoomActor source, IRoomActor target, int timeToLive, int damage,
            string name, int energy)
        {
            TimeToLive = timeToLive;
            BaseDamage = damage;
            Name = name;

            Source = source;
            Target = target;
            Energy = energy;
        }

        public CreateDelayedAttackActorAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }
        
        public override IEnumerable<TagString> Execute(IRoom room)
        {
            base.Execute(room);
            
            var newActor = new DelayedAttackActor(Source, Target, TimeToLive, BaseDamage, Name);
            room.Entities.Add(newActor);

            if (Source is CommandShip)
            {
                return new List<TagString>()
                {
                    new TagString()
                    {
                        Text = ("You fire a < >.").Encode(Name, newActor.Id, LinkColors.HostileEntity),
                        Tags = ActionTags
                    }
                };
            }
            
            return new List<TagString>()
            {
                new TagString()
                {
                    Text = ("You detect an incomming < >.").Encode(Name, newActor.Id, LinkColors.HostileEntity)
                }
            };
        }
    }
}