﻿using System.Collections.Generic;

namespace Models.Dtos
{
    public class GameContentDto
    {
        public IEnumerable<FlexData> Hazards { get; private set; }
        public IEnumerable<FlexData> Mobs { get; private set; }
        public IEnumerable<FlexData> Gatherables { get; private set; }

        public GameContentDto(IEnumerable<FlexData> hazards,
            IEnumerable<FlexData> mobs,
            IEnumerable<FlexData> gatherables)
        {
            Hazards = hazards;
            Mobs = mobs;
            Gatherables = gatherables;
        }
    }
}