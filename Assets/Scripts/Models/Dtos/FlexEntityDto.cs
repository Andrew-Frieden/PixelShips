using System.Collections.Generic;

namespace Models.Dtos
{
    public class FlexEntityDto
    {
        public string Id;
        public string EntityType;
        public string Name;
        public Dictionary<string, int> Stats;
        ABContentDto Content;
    }
}
