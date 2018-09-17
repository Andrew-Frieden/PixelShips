using System.Collections.Generic;
using System.Linq;
using TextEncoding;

namespace Models.Actions
{
    public class GiveASpeechAction : SimpleAction
    {
        public GiveASpeechAction(IRoomActor source)
        {
            Source = source;
            Stats = new Dictionary<string, int>();
        }

        public override IEnumerable<string> Execute(IRoom room)
        {
            return new List<string>() { GetSpeechText().Encode(Source.GetLinkText(), Source.Id, LinkColors.Player) };
        }

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