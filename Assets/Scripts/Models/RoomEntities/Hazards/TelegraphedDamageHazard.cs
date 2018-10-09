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
            return null;
        }
        
        public TelegraphedDamageHazard(FlexEntityDto dto, IRoom room) : base(dto, room)
        {
        }

        public TelegraphedDamageHazard(Dictionary<string, int> stats, Dictionary<string, string> values) : base(stats, values)
        {
            IsHostile = true;
        }

        private class TelegraphedDamageAction : SimpleAction
        {
            private int BaseDamage
            {
                get { return Stats[StatKeys.BaseDamageKey]; }
                set { Stats[StatKeys.BaseDamageKey] = value; }
            }

            public TelegraphedDamageAction(SimpleActionDto dto, IRoom room) : base(dto, room)
            {
            }

            public TelegraphedDamageAction(IRoomActor source, IRoomActor target, int amount, int timeToLive, string telegraphText, string flavorText)
            {
                Source = source;
                Target = target;

                Stats = new Dictionary<string, int>
                {
                    [StatKeys.BaseDamageKey] = amount,
                    [StatKeys.TimeToLiveKey] = timeToLive,
                };

                Values = new Dictionary<string, string>
                {
                    [ValueKeys.HazardDamageText] = flavorText,
                    [ValueKeys.TelegraphDamageText] = flavorText
                };
            }

            public override IEnumerable<TagString> Execute(IRoom room)
            {
                Stats[StatKeys.TimeToLiveKey]--;

                if (Stats[StatKeys.TimeToLiveKey] == 2)
                {
                    return new List<TagString>()
                    {
                        new TagString()
                        {
                            Text = Values[ValueKeys.TelegraphDamageText].Encode(Source.GetLinkText(), Source.Id, LinkColors.Hazard),
                            Tags = new List<EventTag> { }
                        }
                    };
                }
                
                if (Stats[StatKeys.TimeToLiveKey] == 1)
                {
                    var actualDamage = BaseDamage;
                    
                    Target.TakeDamage(actualDamage);
            
                    var resultText = string.Format(Values[ValueKeys.HazardDamageText], actualDamage);
            
                    return new List<TagString>()
                    {
                        new TagString()
                        {
                            Text = resultText.Encode(Source.GetLinkText(), Source.Id, LinkColors.Hazard),
                            Tags = new List<EventTag> { EventTag.Damage }
                        }
                    };
                }
                
                return new DoNothingAction(Source).Execute(room);
            }
        }
    }
}