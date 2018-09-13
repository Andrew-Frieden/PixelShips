using System.Collections.Generic;
using TextEncoding;
using Links.Colors;

namespace Models.Actions
{
    public class WarpDriveReadyAction : SimpleAction
    {
        public WarpDriveReadyAction(CommandShip source)
        {
            Source = source;
        }
        
        public override IEnumerable<string> Execute(IRoom room)
        {
            ((CommandShip) Source).WarpDriveReady = true;

            return new List<string> { "<> is ready.".Encode("Warp drive", Source.Id, LinkColors.Player) };
        }
    }
}