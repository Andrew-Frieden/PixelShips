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
    ABCancel = 0,
    ACancel = 1,
    Cancel = 2
}