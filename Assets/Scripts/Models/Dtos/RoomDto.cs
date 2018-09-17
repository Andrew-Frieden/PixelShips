using System.Collections.Generic;

namespace Models.Dtos
{
    public class RoomDto
    {
        public string Description;
        public RoomFlavor Flavor;
        public string Link;
        public string Id;
    }

    public static partial class DtoHelpers
    {
        public static IEnumerable<ABContentDto> GetContent(this RoomDto dto)
        {
            var contents = new List<ABContentDto>();

            //dto.Mobs.ForEach(m => contents.Add(m.Content));

            return contents;
        }
    }
}
