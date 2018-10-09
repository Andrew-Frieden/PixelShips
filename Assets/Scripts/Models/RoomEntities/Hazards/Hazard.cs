using System.Collections.Generic;
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
        
        public Hazard(FlexEntityDto dto, IRoom room) : base(dto, room)
        {
        }
        
        public Hazard(Dictionary<string, int> stats, Dictionary<string, string> values) : base(stats, values)
        {
            IsHostile = true;
        }
    }
}