namespace TextSpace.Models.Dtos
{
    public class GameContentFilterDto
    {
        public RoomFlavor RoomFlavor { get; private set; }
        public RoomActorFlavor RoomActorFlavor { get; private set; }

        public GameContentFilterDto(RoomFlavor roomFlavor, RoomActorFlavor roomActorFlavor)
        {
            RoomFlavor = roomFlavor;
            RoomActorFlavor = roomActorFlavor;
        }
    }
}