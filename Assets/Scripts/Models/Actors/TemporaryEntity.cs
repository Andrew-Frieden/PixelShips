namespace Models.Actors
{
    public abstract class TemporaryEntity : FlexEntity
    {
        public const string TimeToLiveKey = "timetolive";

        protected TemporaryEntity() : base()
        {
        }

        public override void AfterAction(IRoom room)
        {
            if (Stats[TimeToLiveKey] == 0)
            {
                room.Entities.Remove(this);
            }
        }
    }
}