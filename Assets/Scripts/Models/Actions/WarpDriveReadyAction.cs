using System.Collections.Generic;

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

            return new List<string> { "Your drive is ready." };
        }
    }
}