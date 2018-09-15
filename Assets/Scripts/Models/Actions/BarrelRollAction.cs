using Links.Colors;
using System.Collections.Generic;
using System.Linq;
using TextEncoding;

namespace Models.Actions
{
    public class BarrellRollAction : SimpleAction
    {
        public BarrellRollAction(IRoomActor source)
        {
            Source = source;
            Stats = new Dictionary<string, int>();
        }

        public override IEnumerable<string> Execute(IRoom room)
        {
            return new List<string>() { GetRollText().Encode(Source.GetLinkText(), Source.Id, LinkColors.Player) };
        }

        private string GetRollText()
        {
            return RollTexts.OrderBy(t => System.Guid.NewGuid()).First();
        }

        private IEnumerable<string> RollTexts => new string[]
        {
            "<> engage thrusters and try to keep moving.",
            "<> throw the ship into an evasive action.",
            "<> attempt to disengage."
        };
    }
}