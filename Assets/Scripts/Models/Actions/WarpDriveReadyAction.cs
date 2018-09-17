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
        
        public override IEnumerable<StringTagContainer> Execute(IRoom room)
        {
            ((CommandShip) Source).WarpDriveReady = true;
            
            return new List<StringTagContainer>()
            {
                new StringTagContainer()
                {
                    Text = "<> is ready.".Encode("Warp drive", room.Id, LinkColors.Room),
                    ResultTags = new List<ActionResultTags> { }
                }
            };
        }
    }
}