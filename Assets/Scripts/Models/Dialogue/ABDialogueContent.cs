using Models.Actions;

public class ABDialogueContent
{
    public string MainText;
    public string OptionAText;
    public string OptionBText;
    public IRoomAction OptionAAction;
    public IRoomAction OptionBAction;
    public ABDialogueMode Mode;
}

public enum ABDialogueMode
{
    AB,
    AorCancel,
    Acknowledge
}