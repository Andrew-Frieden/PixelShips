using Models;
using Models.Stats;
using System.Collections.Generic;

namespace GameData
{
    public static class ExampleGameData
    {
        public static List<FlexData> SometimesDamageHazards => new List<FlexData>
        {
            new FlexData()
            {
                Values = new Dictionary<string, string>
                {
                    { ValueKeys.Name, "Plasma Storm" },
                    { ValueKeys.DialogueText, "The <> looks pretty dangerous. There isn't much you can do about it." },
                    { ValueKeys.LookText, "A <> crackles through this sector." },
                    { ValueKeys.HazardDamageText, "An energy surge from a <> scorches your hull for {0} damage!" }
                },
                Stats = new Dictionary<string, int>
                {
                    { StatKeys.HazardDamageAmount, 2 },
                    { StatKeys.HazardDamageChance, 75 }
                }
            },
            new FlexData()
            {
                Values = new Dictionary<string, string>
                {
                    { ValueKeys.Name, "Dense Kelp Tangle" },
                    { ValueKeys.DialogueText, "The wavy field of <> is filled with razor sharp stalks as far as your scanners can detect." },
                    { ValueKeys.LookText, "An undulating <> will be hard to navigate." },
                    { ValueKeys.HazardDamageText, "A tentacle surges from a <> and tears into your hull for {0} damage!" }
                },
                Stats = new Dictionary<string, int>
                {
                    { StatKeys.HazardDamageAmount, 3 },
                    { StatKeys.HazardDamageChance, 50 }
                }
            },
            new FlexData()
            {
                Values = new Dictionary<string, string>
                {
                    { ValueKeys.Name, "Asteroid Field" },
                    { ValueKeys.DialogueText, "An uncountable number of masses from the <> whirl through space. It looks like you could get hit at any time." },
                    { ValueKeys.LookText, "The sector is saturated with an <>" },
                    { ValueKeys.HazardDamageText, "Something massive from the <> smashes into your hull for {0} damage!" }
                },
                Stats = new Dictionary<string, int>
                {
                    { StatKeys.HazardDamageAmount, 6 },
                    { StatKeys.HazardDamageChance, 25 }
                }
            },
        };
    }
}