using System.Collections.Generic;

public class CmdState
{
    IRoom CurrentRoom;
    //  playership object

    public List<string> ResolveNext(IRoomAction playerAction)
    {
        var actions = new List<IRoomAction>() { playerAction };
        CurrentRoom.Entities.ForEach(e => actions.Add(e.GetNextAction(this)));

        var resultText = new List<string>();
        actions.ForEach(a => resultText.AddRange(a.Execute(this)));

        return resultText;
    }
}