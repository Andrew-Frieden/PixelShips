using System.Collections.Generic;

namespace Models.Dtos
{
    public class RoomDto
    {
        public string Description;
        public RoomFlavor Flavor;
        public string Link;
        public string Id;

        public ShipDto PlayerShip;

        public List<MobDto> Mobs;
        //public List<NPCDto> Npcs;
        //public List<HazardDto> Hazards;
    }

    public static partial class DtoHelpers
    {
        public static IEnumerable<ABContentDto> GetContent(this RoomDto dto)
        {
            var contents = new List<ABContentDto>();

            dto.Mobs.ForEach(m => contents.Add(m.Content));
            contents.Add(dto.PlayerShip.ContentDto);

            return contents;
        }
    }
}
