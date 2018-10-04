using System.Collections.Generic;
using Models.Actions;
using Models.Dtos;
using Models.Stats;
using TextEncoding;

namespace Models.RoomEntities.Mobs
{
    public abstract class Mob : FlexEntity
    {
        public override TagString GetLookText()
        {
            if (IsHostile)
            {
                return new TagString()
                {
                    Text = Values[ValueKeys.LookTextAggro].Encode(Name, Id, LinkColors.HostileEntity),
                };
            }
        
            return new TagString()
            {
                Text = Values[ValueKeys.LookText].Encode(Name, Id, LinkColors.CanCombatEntity),
            };
        }
        
        protected Mob() { }

        protected Mob(FlexEntityDto dto, IRoom room) : base(dto, room) { }

        protected Mob(Dictionary<string, int> stats, Dictionary<string, string> values) : base(stats, values) { }
    }
}