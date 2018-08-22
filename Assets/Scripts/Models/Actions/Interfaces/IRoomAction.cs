using System.Collections.Generic;
using Models;

public interface IRoomAction
{
    IEnumerable<string> Execute(IRoom room);
}