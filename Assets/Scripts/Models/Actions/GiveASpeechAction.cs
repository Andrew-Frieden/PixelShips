using TextSpace.Models.Dtos;
using System.Collections.Generic;
using System.Linq;
using TextEncoding;

namespace TextSpace.Models.Actions
{
    public class GiveASpeechAction : SimpleAction
    {
        public GiveASpeechAction(IRoomActor source)
        {
            Source = source;
            Stats = new Dictionary<string, int>();
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            return new List<TagString>()
            {
                new TagString()
                {
                    Text = GetSpeechText().Encode(Source.GetLinkText(), Source.Id, LinkColors.Player),
                    Tags = new List<UIResponseTag> { }
                }
            };
        }

        public GiveASpeechAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }

        private string GetSpeechText()
        {
            return SpeechTexts.OrderBy(t => System.Guid.NewGuid()).First();
        }

        private IEnumerable<string> SpeechTexts => new string[]
        {
            "<> say something very inspiring.",
            "<> come up with a ingenious plan.",
            "<> muster all your cunning."
        };
    }
}