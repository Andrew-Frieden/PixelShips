using TextSpace.Models.Actions;

namespace TextSpace.Models
{
    public interface ITextEntity
    {
        string Id { get; }
        TagString GetLookText();
        string GetLinkText(); 
        ABDialogueContent DialogueContent { get; set; }
    }
}