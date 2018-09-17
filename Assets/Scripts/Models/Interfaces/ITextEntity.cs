using Models.Actions;

namespace Models
{
    public interface ITextEntity
    {
        string Id { get; }
        StringTagContainer GetLookText();
        string GetLinkText();
        ABDialogueContent DialogueContent { get; set; }
    }
}