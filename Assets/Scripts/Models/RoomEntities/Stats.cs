using System.Collections.Generic;

namespace Models.Stats
{
    public static partial class StatKeys
    {
        #region Core

        public const string IsDestroyed = "destroyed";
        public const string IsAttackable = "attackable"; //  1 if the entity is capable of participating in combat
        public const string IsHostile = "hostile";     //  1 if the entity is actively attacking or being attacked
        public const string Hull = "current_hull";
        public const string MaxHull = "max_hull";
        public const string Captainship = "captainship";
        public const string DamageMitigationStat = "damage_mitigation";
        public const string HazardDamageAmount = "hazard_damage_amount";
        public const string HazardDamageChance = "hazard_damage_chance";
        public const string MaxShields = "max_shields";
        public const string Shields = "current_shields";
        public const string MaxHardwareSlots = "max_flex_slots";
        public const string Energy = "energy";
        public const string MaxEnergy = "max_energy";

        #endregion
        
        #region Actions
        
        public const string TimeToLiveKey = "timetolive";
        public const string BaseDamageKey = "base_damage";
        
        #endregion

        #region Cargo

        public const string Credits = "credits";
        public const string Resourcium = "resourcium";
        public const string Scrap = "scrap";
        public const string Techanite = "techanite";
        public const string MachineParts = "machine_parts";
        public const string PulsarCoreFragments = "pulsar_core_fragment";
            
        #endregion
    }

    public static class ValueKeys
    {
        public static readonly string Name = "name";
        public static readonly string LookText = "look_text";
        public static readonly string LookTextAggro = "look_text_aggro";
        public static readonly string DialogueText = "dialogue_text";
        public static readonly string HazardDamageText = "hazard_damage_text";
        public static readonly string TelegraphDamageText = "telegraph_damage_text";

        public static readonly string LightWeapon = "light_weapon";
        public static readonly string HeavyWeapon = "heavy_weapon";
    }

    public static class StatsHelper
    {
        public static Dictionary<string, int> EmptyStatsBlock()
        {
            return new Dictionary<string, int>
            {
                { StatKeys.IsHostile, 0 },
                { StatKeys.IsAttackable, 0 },
                { StatKeys.Hull, 0 },
                { StatKeys.Captainship, 0 },
            };
        }
    }
}
