using System;
using EnumerableExtensions;

namespace Models.Factories
{
    public static class RoomFactoryHelper
    {
        private static float CHANCE_EVEN_PURCHASE_SPLIT = 0.50f;
        
        private static float CHANCE_ONE_ENTITY = 0.50f;
        private static float CHANCE_EXACTLY_TWO_ENTITY = 0.90f;
        
        public static IRoomActor FromFlexData(this FlexData data)
        {
            return (IRoomActor)Activator.CreateInstance(Type.GetType(data.EntityType), new object[] { data });
        }

        public static int GetEntityCount()
        {
            if (CHANCE_ONE_ENTITY.Rng())
            {
                return 1;
            }

            return CHANCE_EXACTLY_TWO_ENTITY.Rng() ? 2 : 3;
        }

        public static int CalculateTotalPurchasingPower(int missionCount, int additionalMissionDifficulty, int additionalRoomDifficulty)
        {
            return 5 * missionCount + additionalMissionDifficulty + additionalRoomDifficulty;
        }

        public static decimal[] CalculateIndividualPurchasingPower(int totalPurchasingPower, int entityCount)
        {
            if (entityCount == 2)
            {
                if (CHANCE_EVEN_PURCHASE_SPLIT.Rng())
                {
                    return new[] { (decimal) totalPurchasingPower/2, (decimal) totalPurchasingPower/2 };
                }
            
                return new[] { totalPurchasingPower * (decimal) 0.75, (decimal) totalPurchasingPower/4 };
            }

            if (entityCount == 3)
            {
                if (CHANCE_EVEN_PURCHASE_SPLIT.Rng())
                {
                    return new[] { (decimal) totalPurchasingPower/3, (decimal) totalPurchasingPower/3, (decimal) totalPurchasingPower/3 };
                }
            
                return new[] { totalPurchasingPower * (decimal) 0.75, (decimal) totalPurchasingPower/4 };
            }

            return new[] { (decimal) 1 };
        }
    }
}