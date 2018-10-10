using System.Collections.Generic;
using Helpers;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
using TextEncoding;

namespace Models.RoomEntities.Hazards
{
    public abstract class Hazard : FlexEntity
    {
        public override void CalculateDialogue(IRoom room)
        {
            DialogueContent = DialogueBuilder.Init()
                .AddMainText(Values[ValueKeys.DialogueText].Encode(Name, Id, LinkColors.Hazard))
                .Build();
        }

        public override TagString GetLookText()
        {
            return new TagString()
            {
                Text = Values[ValueKeys.LookText].Encode(Name, Id, LinkColors.Hazard)
            };
        }
        
        public Hazard(FlexEntityDto dto) : base(dto) { }
        
        public Hazard(FlexData data) : base(data)
        {
            IsHostile = true;
        }
        
        protected class HazardDamageAction : SimpleAction
        {
            private int BaseDamage
            {
                get
                {
                    return Stats[StatKeys.BaseDamageKey];
                }
                set
                {
                    Stats[StatKeys.BaseDamageKey] = value;
                }
            }

            public HazardDamageAction(SimpleActionDto dto, IRoom room) : base(dto, room)
            {
            }

            public HazardDamageAction(IRoomActor source, IRoomActor target, int amount, string flavorText)
            {
                Source = source;
                Target = target;

                Stats = new Dictionary<string, int>
                {
                    [StatKeys.BaseDamageKey] = amount
                };

                Values = new Dictionary<string, string>
                {
                    [ValueKeys.HazardDamageText] = flavorText
                };
            }

            public override IEnumerable<TagString> Execute(IRoom room)
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
        }
    }
}