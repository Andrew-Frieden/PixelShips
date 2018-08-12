public interface IRoomEntity
{
    string Id { get; }

    string GetLookText();
    string GetLinkText();

    ABDialogueContent GetInteraction(CmdState s);
    IRoomAction GetNextAction(CmdState s);
}