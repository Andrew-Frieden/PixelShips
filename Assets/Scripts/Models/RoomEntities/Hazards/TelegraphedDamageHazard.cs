using System.Collections.Generic;
using Helpers;
using Models.Actions;
using Models.Dtos;
using Models.Stats;
using TextEncoding;

namespace Models.RoomEntities.Hazards
{
    public class TelegraphedDamageHazard : Hazard
    {
        public override IRoomAction MainAction(IRoom room)
        {
            Stats[StatKeys.TimeToLiveKey]--;

            if (Stats[StatKeys.TimeToLiveKey] == 2)
            {
                return new TelegraphedDamageAction(this, room.PlayerShip, Values[ValueKeys.TelegraphDamageText]);
            }
                
            if (Stats[StatKeys.TimeToLiveKey] == 1)
            {
                IsDestroyed = true;
                return new HazardDamageAction(this, room.PlayerShip, Stats[StatKeys.HazardDamageAmount], Values[ValueKeys.HazardDamageText]);
            }
                
            return new DoNothingAction(this);
        }
        
        public TelegraphedDamageHazard(FlexEntityDto dto) : base(dto) { }

        public TelegraphedDamageHazard(Dictionary<string, int> stats, Dictionary<string, string> values) : base(stats, values)
        {
            IsHostile = true;
        }

        private class TelegraphedDamageAction : SimpleAction
        {
            private readonly string _telegraphText;

            public TelegraphedDamageAction(SimpleActionDto dto, IRoom room) : base(dto, room)
            {
            }

            public TelegraphedDamageAction(IRoomActor source, IRoomActor target, string telegraphText)
            {
                Source = source;
                Target = target;

                _telegraphText = telegraphText;
            }

            public override IEnumerable<TagString> Execute(IRoom room)
            {
                return new List<TagString>()
                {
                    new TagString()
                    {
                        Text = _telegraphText.Encode(Source.GetLinkText(), Source.Id, LinkColors.Hazard),
                        Tags = new List<EventTag> { }
                    }
                };
            }
        }
    }
}