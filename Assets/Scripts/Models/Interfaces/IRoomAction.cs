using System.Collections.Generic;

public interface IRoomAction
{
    List<string> Execute(CmdState s);
}