using System;
using System.Collections.Generic;

namespace Models
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
}
