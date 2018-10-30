using System;
using TextSpace.Models;
using TextSpace.Models.Stats;

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

        public static string ResourceDisplayText(this string resourceKey)
        {
            switch (resourceKey)
            {
                case "scrap":
                    return "Resourcium";
                case "resourcium":
                    return "Resourcium";
                case "techanite":
                    return "Resourcium";
                case "machine_parts":
                    return "Resourcium";
                case "pulsar_core_fragment":
                    return "Resourcium";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}