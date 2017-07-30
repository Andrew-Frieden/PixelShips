using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelSpace.Models.SharedModels.Ships
{
    public class Ship
    {
        public int? Version { get; set; }
        public string Id { get; set; }

        public string Name { get; set; }
        public int Hull { get; set; }
        public int MaxHull { get; set; }
        public DateTime LastJump { get; set; }

        public Room Room { get; set; }

        public const double JUMP_COOLDOWN = 5;
        public bool CanJump
        {
            get
            {
                return (DateTime.UtcNow - LastJump).TotalSeconds > JUMP_COOLDOWN;
            }
        }
    }
}
