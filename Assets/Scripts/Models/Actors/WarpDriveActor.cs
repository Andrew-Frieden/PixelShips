using Models.Actions;

namespace Models.Actors
{
    public class WarpDriveActor : DelayedActor
    {
        public override string GetLookText()
        {
            throw new System.NotImplementedException();
        }

        public override string GetLinkText()
        {
            throw new System.NotImplementedException();
        }

        public override IRoomAction GetNextAction(IRoom s)
        {
            if (Stats[TimeToLiveKey] == 1)
            {
                return new WarpAction();
            }
            else
            {
                return new DelayedAction($"Your warp drive is charging. It will be ready in ", Id);
            }
        }
    }
}