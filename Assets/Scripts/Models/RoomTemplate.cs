namespace Models
{
    public class RoomTemplate
    {
        public int Difficulty { get; }
        public RoomFlavor Flavor { get; }
        public string Mechanic { get; }

        public RoomTemplate(int difficulty, RoomFlavor flavor, string mechanic)
        {
            Difficulty = difficulty;
            Flavor = flavor;
            Mechanic = mechanic;
        }
    }
}