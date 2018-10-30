using System.Collections.Generic;
using TextSpace.Models.Actors;
using TextSpace.Models.Dtos;
using TextSpace.Models.Stats;
using TextEncoding;

namespace TextSpace.Models.Actions
{
    public class CreateShieldActorAction : SimpleAction
    {
        protected int DamageReduction
        {
            get
            {
                return Stats[StatKeys.DamageReduction];
            }
            set
            {
                Stats[StatKeys.DamageReduction] = value;
            }
        }

        public CreateShieldActorAction(IRoomActor source, IRoomActor target, int timeToLive, int damageReduction)
        {
            TimeToLive = timeToLive;
            DamageReduction = damageReduction; 

            Source = source;
            Target = target;
        }

        public CreateShieldActorAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }
        
        public override IEnumerable<TagString> Execute(IRoom room)
        {
            room.Entities.Add(new ShieldActor(Source, Target, TimeToLive, DamageReduction));
            
            return new List<TagString>()
            {
                new TagString()
                {
                    Text = "<> generate a shield.".Encode("You", Source.Id, "blue"),
                    Tags = new List<UIResponseTag> { }
                }
            };
        }
    }
}