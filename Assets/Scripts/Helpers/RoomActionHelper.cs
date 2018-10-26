using System;
using Items;
using Models;
using Models.Stats;

namespace Helpers
{
    public static class RoomActionHelper
    {
        public static int TakeDamage(this IRoomActor actor, int damage)
        {
            var overflow = 0;
            
            if (actor.Stats.ContainsKey(StatKeys.DamageMitigationStat))
            {
                damage -= actor.Stats[StatKeys.DamageMitigationStat];
                damage = Math.Max(damage, 1);
            }

            if (actor.Stats.ContainsKey(StatKeys.Shields))
            {
                if (actor.Stats[StatKeys.Shields] > damage)
                {
                    actor.Stats[StatKeys.Shields] -= damage;
                }
                else
                {
                    overflow = damage - actor.Stats[StatKeys.Shields];
                    actor.Stats[StatKeys.Shields] = 0;
                }
            }
            else
            {
                overflow = damage;
            }
            
            actor.Stats[StatKeys.Hull] -= overflow;

            if (actor.Stats[StatKeys.Hull] <= 0)
            {
                actor.IsDestroyed = true;
            }

            return damage;
        }

        public static int HealDamage(this IRoomActor actor, int baseHeal)
        {
            var heal = baseHeal;
            if (actor.Stats[StatKeys.Shields] + baseHeal > actor.Stats[StatKeys.MaxShields])
            {
                heal = actor.Stats[StatKeys.MaxShields] - actor.Stats[StatKeys.Shields];
            }
            actor.Stats[StatKeys.Shields] += heal;
            return heal;
        }

        public static Loot LootGatherable(this IRoomActor actor)
        {
            var lootDrop = UnityEngine.Random.Range(0, 10);

            string lootType;
            int lootAmount = 0;

            switch (lootDrop)
            {
                case 0:
                    lootType = StatKeys.Resourcium;
                    lootAmount = 15;
                    break;
                case 1:
                    lootType = StatKeys.Resourcium;
                    lootAmount = 5;
                    break;
                case 2:
                    lootType = StatKeys.Scrap;
                    lootAmount = 15;
                    break;
                case 3:
                    lootType = StatKeys.Resourcium;
                    lootAmount = 25;
                    break;
                case 4:
                    lootType = StatKeys.Scrap;
                    lootAmount = 25;
                    break;
                case 5:
                    lootType = StatKeys.Scrap;
                    lootAmount = 50;
                    break;
                case 6:
                    lootType = StatKeys.Scrap;
                    lootAmount = 75;
                    break;
                case 7:
                    lootType = StatKeys.Scrap;
                    lootAmount = 100;
                    break;
                case 8:
                    lootType = StatKeys.Resourcium;
                    lootAmount = 1;
                    break;        
                default:
                    lootType = StatKeys.Scrap;
                    lootAmount = 1;
                    break;        
            }

            if (actor is CommandShip)
            {
                var cmdShip = (CommandShip) actor;
                var boosters = cmdShip.GetHardware<GatheringBoost>();

                foreach (var boost in boosters)
                    boost.ApplyBoost(ref lootAmount);
            }

            actor.Stats[lootType] += lootAmount;

            return new Loot(lootAmount, lootType);
        }
    }

    public class Loot
    {
        public int Amount { get; }
        public string Type { get; }
        
        public Loot(int amount, string type)
        {
            Amount = amount;
            Type = type;
        }
    }
}