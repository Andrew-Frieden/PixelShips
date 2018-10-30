namespace TextSpace.Models.Stats
{
    public static partial class StatKeys
    {
        #region Core

        public const string IsDestroyed = "destroyed";
        public const string IsAttackable = "attackable"; //  1 if the entity is capable of participating in combat
        public const string IsHostile = "hostile";     //  1 if the entity is actively attacking or being attacked
        public const string IsHidden = "hidden";
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
        
        public const string TimeToLive = "time_to_live";
        public const string BaseDamage = "base_damage";
        public const string DamageReduction = "damage_reduction";
        public const string BaseHeal = "base_heal";

        #endregion

        #region NPCs
        
        public const string ScrapToResourcium = "scrap_to_resourcium";
        
        #endregion

        #region Cargo

        public const string Gathered = "gathered";
        public const string Credits = "credits";
        public const string Resourcium = "resourcium";
        public const string Scrap = "scrap";
        public const string Techanite = "techanite";
        public const string MachineParts = "machine_parts";
        public const string PulsarCoreFragments = "pulsar_core_fragment";
        
        public const string SmallestAmount = "smallest_amount";
        public const string LargestAmount = "largest_amount";
        
        public const string DepletionChance = "depletion_chance";
        
        #endregion
        
        #region Items
        
        public const string WeaponType = "weapon_type";

        public static readonly string[] WeaponWeights = {
            "weapon_id_0_weight",
            "weapon_id_1_weight",
            "weapon_id_2_weight",
            "weapon_id_3_weight",
            "weapon_id_4_weight"
        };
        
        #endregion

        #region CommandShip

        public const string WarpDriveReady = "warp_drive_ready";

        #endregion
    }

    public static class ValueKeys
    {
        public const string Name = "name";
        public const string LookText = "look_text";
        public const string LookTextAggro = "look_text_aggro";
        public const string DialogueText = "dialogue_text";
        public const string HazardDamageText = "hazard_damage_text";
        public const string TelegraphDamageText = "telegraph_damage_text";

        public const string DependentActorId = "dependent_actor_id";

        public const string LightWeapon = "light_weapon";
        public const string HeavyWeapon = "heavy_weapon";
        
        public const string WeaponId = "weapon_id";
        
        public const string CaptainName = "captain_name";

        public const string NpcDealA = "npc_deal_a";
        public const string NpcDealB = "npc_deal_b";

        public const string SourceId = "source_id";
        public const string TargetId = "target_id";
        
        public const string ResourceKey = "resource_key";

        public static string DialogueStateText(string state)
        {
            return $"{DialogueText}_{state}";
        }
        
        public static readonly string[] WeaponIds = {
            "weapon_id_0",
            "weapon_id_1",
            "weapon_id_2",
            "weapon_id_3",
            "weapon_id_4"
        };
    }
}
