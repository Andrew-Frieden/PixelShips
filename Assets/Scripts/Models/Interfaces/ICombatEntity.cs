namespace Models
{
    public interface ICombatEntity
    {
        string Id { get; }
        int Hull { get; set; }
    }
}