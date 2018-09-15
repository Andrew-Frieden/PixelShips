using System.Collections.Generic;

namespace Models.Stats
{
    public static class StatKeys
    {
        #region Core
    
        public const string CanCombat = "can_combat"; //  1 if the entity is capable of participating in combat
        public const string IsAggro = "is_aggro";     //  1 if the entity is actively attacking or being attacked
        public const string Hull = "current_hull";
        public const string MaxHull = "max_hull";
        public const string Captainship = "captainship";
        public const string ExampleDamageMitigationStat = "damage_mitigation";
        public const string HazardDamageAmount = "hazard_damage_amount";
        public const string HazardDamageChance = "hazard_damage_chance";
        public const string MaxShields = "max_shields";
        public const string Shields = "current_shields";

        #endregion

        #region Cargo

        public const string Resourcium = "resourcium";
        public const string Scrap = "scrap";
        public const string Techanite = "techanite";
        public const string MachineParts = "machine_parts";
        public const string PulsarCoreFragments = "pulsar_core_fragment";
            
        #endregion
    }

    public static class ValueKeys
    {
        public const string Name = "name";
        public const string LookText = "look_text";
        public const string DialogueText = "dialogue_text";
        public const string HazardDamageText = "hazard_damage_text";
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
