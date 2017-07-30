using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.SpaceUpdates
{
    public static class UpdateHelpers
    {
        public static ShipUpdate FromDto(this ShipUpdateDto dto)
        {
            return new ShipUpdate
            {
                Text = dto.Text,
                UniverseTime = dto.UniverseTime,
                UpdateId = dto.UpdateId
            };
        }

        public static ShipUpdateDto ToDto(this ShipUpdate update)
        {
            return new ShipUpdateDto
            {
                Text = update.Text,
                UniverseTime = update.UniverseTime,
                UpdateId = update.UpdateId
            };
        }
    }
}
