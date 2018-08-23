namespace Models
{
    public interface ICombatEntity : ITextEntity
    {
        int Hull { get; set; }
    }
}