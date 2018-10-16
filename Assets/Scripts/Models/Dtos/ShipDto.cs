using System.Collections.Generic;

namespace Models.Dtos
{
    public class ShipDto
    {
        public string Id;
        public Dictionary<string, int> Stats;
        public Dictionary<string, string> Values;
        public List<FlexEntityDto> HardwareData;
        public FlexEntityDto LightWeapon;
        public FlexEntityDto HeavyWeapon;
        public ABContentDto ContentDto;
    }
}
