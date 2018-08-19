namespace Models
{
    public interface ITextEntity
    {
        string Id { get; }
        string GetLookText();
        string GetLinkText();
    }
}