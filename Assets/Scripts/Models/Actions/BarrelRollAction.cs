using TextSpace.Models.Dtos;
using System.Collections.Generic;
using System.Linq;
using TextEncoding;

namespace TextSpace.Models.Actions
{
    public class BarrellRollAction : SimpleAction
    {
        public BarrellRollAction(IRoomActor source)
        {
            Source = source;
            Stats = new Dictionary<string, int>();
        }

        public BarrellRollAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            return new List<TagString>()
            {
                new TagString()
                {
                    Text = GetRollText().Encode(Source.GetLinkText(), Source.Id, LinkColors.Player),
                    Tags = new List<UIResponseTag> { }
                }
            };
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