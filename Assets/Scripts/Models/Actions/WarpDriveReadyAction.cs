using Models.Dtos;
using System.Collections.Generic;
using TextEncoding;

namespace Models.Actions
{
    public class WarpDriveReadyAction : SimpleAction
    {
        public WarpDriveReadyAction(CommandShip source)
        {
            Source = source;
        }

        public WarpDriveReadyAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }
        
        public override IEnumerable<TagString> Execute(IRoom room)
        {
            ((CommandShip) Source).WarpDriveReady = true;
            
            return new List<TagString>()
            {
                new TagString()
                {
                    Text = "<> is ready.".Encode("Warp drive", room.Id, LinkColors.Room),
                    Tags = new List<EventTag> { }
                }
            };
        }
    }
}