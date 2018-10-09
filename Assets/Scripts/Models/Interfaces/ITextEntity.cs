using Models.Actions;

namespace Models
{
    public interface ITextEntity
    {
        string Id { get; }
        TagString GetLookText();
        string GetLinkText(); 
        ABDialogueContent DialogueContent { get; set; }
    }
}