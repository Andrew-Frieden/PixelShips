public class ABDialogueContent
{
    public string MainText;
    public string OptionAText;
    public string OptionBText;
    public IRoomAction OptionAAction;
    public IRoomAction OptionBAction;
}

public enum ABDialogueMode
{
    AB,
    A,
    Forced
}