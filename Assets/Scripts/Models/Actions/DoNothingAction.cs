using System.Collections.Generic;
using Models.Dtos;

namespace Models.Actions
{
    public class DoNothingAction : SimpleAction
    {
        public DoNothingAction(SimpleActionDto dto, IRoom room)
        {
        }
        
        public DoNothingAction(IRoomActor source)
        {
            Source = source;
        }
    
        public override IEnumerable<TagString> Execute(IRoom room)
        {
            return new List<TagString>() { };
        }
    }
}
