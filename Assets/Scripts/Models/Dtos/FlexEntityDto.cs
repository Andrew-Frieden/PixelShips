using System.Collections.Generic;

namespace TextSpace.Models.Dtos
{
    public class FlexEntityDto
    {
        public string Id;
        public string EntityType;
        public Dictionary<string, int> Stats;
        public Dictionary<string, string> Values;
        public ABContentDto ContentDto;
    }
}
