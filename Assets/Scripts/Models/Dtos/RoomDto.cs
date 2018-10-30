﻿using System.Collections.Generic;

namespace TextSpace.Models.Dtos
{
    public class RoomDto
    {
        public string Description;
        public RoomFlavor Flavor;
        public string Id;
        public List<FlexEntityDto> Entities;
        public string Name;
        public string LookText;
        public ABContentDto ContentDto;
        public List<RoomTemplateDto> ExitDtos;
    }

    public class RoomTemplateDto
    {
        public int PowerLevel;
        public RoomFlavor Flavor;
        public List<RoomActorFlavor> ActorFlavors;
    }
}
