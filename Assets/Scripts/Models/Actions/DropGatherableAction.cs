using System.Collections.Generic;
using TextEncoding;
using UnityEngine;

namespace Models.Actions
{
    public class DropGatherableAction : SimpleAction
    {
        private readonly BasicGatherable _gatherable;
        
        public DropGatherableAction(IRoomActor src, BasicGatherable gatherable)
        {
            Source = src;
            _gatherable = gatherable;
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {

            var explosion = $"The <>'s hull splinters apart!".Encode(Source, LinkColors.HostileEntity).Tag();


            if (2 >= Random.Range(0, 10))
            {
                return new List<TagString> { explosion };
            }
            
            room.Entities.Add(_gatherable);
            var dropped = $"Earthy <> spews from the wreckage.".Encode(_gatherable, LinkColors.Gatherable).Tag();
            return new List<TagString> { explosion, dropped };
        }
    }
}