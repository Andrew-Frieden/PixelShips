namespace Models
{
    public class RoomTemplate
    {
        private int _difficulty;
        private RoomFlavor _flavor;
        private string _mechanic;

        public RoomTemplate(int difficulty, RoomFlavor flavor, string mechanic)
        {
            _difficulty = difficulty;
            _flavor = flavor;
            _mechanic = mechanic;
        }
    }
}