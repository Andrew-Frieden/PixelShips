using System.Collections.Generic;

namespace Models.Stats
{
    public static class StatKeys
    {
        public static readonly string CanCombat = "can_combat";     //  1 if the entity is capable of participating in combat
        public static readonly string IsAggro = "is_aggro";         //  1 if the entity is actively attacking or being attacked
        public static readonly string Hull = "current_hull";
        public static readonly string MaxHull = "max_hull";
        public static readonly string Captainship = "captainship";
        public static readonly string Resourcium = "resourcium";
        public static readonly string ExampleDamageMitigationStat = "damage_mitigation";
    }

    public static class StatsHelper
    {
        public static Dictionary<string, int> EmptyStatsBlock()
        {
            return new Dictionary<string, int>
            {
                { StatKeys.IsAggro, 0 },
                { StatKeys.CanCombat, 0 },
                { StatKeys.Hull, 0 },
                { StatKeys.Captainship, 0 },
            };
        }
    }
}
