using Models;
using Models.Stats;

namespace Helpers
{
    public static class RoomActorHelper
    {
        public static void TakeDamage(this IRoomActor actor, int damage)
        {
            var overflow = 0;

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
        }
    }
}