using System.Collections.Generic;
using Models;
using Models.Text;

public interface IRoom
{
    CommandShip PlayerShip { get; }
    string Description { get; }
    RoomFlavor Flavor { get; }
    List<IRoomEntity> Entities { get; }

    List<string> ResolveNext(IRoomAction playerAction);
    TextBlock GetLookText();
}