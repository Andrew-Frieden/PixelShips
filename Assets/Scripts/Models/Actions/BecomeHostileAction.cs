using System.Collections.Generic;
using TextEncoding;
using UnityEngine;

namespace Models.Actions
{
    public class BecomeHostileAction : SimpleAction
    {
        private readonly int _chance;
        
        public BecomeHostileAction(IRoomActor src, int chance)
        {
            Source = src;
            _chance = chance;
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            if (Source.IsHostile || (Random.Range(0, 10) < _chance))
            {
                return new List<TagString>();
            }
            
            Source.IsHostile = true;
                
            return new List<TagString>()
            {
                new TagString()
                {
                    Text = "<> warms up their weapon systems".Encode(Source.GetLinkText(), Source.Id, LinkColors.HostileEntity),
                }
            };

        }
    }
}