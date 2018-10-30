using TextSpace.Models.Actions;

public class ABDialogueContent
{
    public string MainText;
    public string OptionAText;
    public string OptionBText;
    public IRoomAction OptionAAction;
    public IRoomAction OptionBAction;
    public ABDialogueMode Mode;

    public void CalculateMode()
    {
        if (OptionAAction != null && OptionBAction != null)
            Mode = ABDialogueMode.ABCancel;
        else if (OptionAAction != null && OptionBAction == null)
            Mode = ABDialogueMode.ACancel;
        else if (OptionAAction == null && OptionBAction == null)
            Mode = ABDialogueMode.Cancel;
    }
}

public enum ABDialogueMode
{
    ABCancel = 0,
    ACancel = 1,
    Cancel = 2
}