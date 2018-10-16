using Models;
using Models.Stats;
using System.Collections.Generic;

namespace GameData
{
    public static class ExampleGameData
    {
        public static List<FlexData> ScrapDealerData => new List<FlexData>
        {
            new FlexData()
            {
                Powerlevel = 1,
                EntityType = "TextSpace.RoomEntities.ScrapDealerNpc",
                ActorFlavors = new RoomActorFlavor[] { RoomActorFlavor.Npc },
                RoomFlavors = new RoomFlavor[] { RoomFlavor.Empty, RoomFlavor.Nebula },
                Values = new Dictionary<string, string>
                {
                    { ValueKeys.Name, "Scrap Dealer" },
                    { ValueKeys.DialogueText, $"The <> trade vessel looks pretty shoddy. Cables and spare parts are everywhere." },
                    { ValueKeys.LookText, "A <> looks to be open for business." }
                },
                Stats = new Dictionary<string, int> 
                {
                    { StatKeys.ScrapToResourcium, 8 }
                }
            },
            new FlexData()
            {
                Powerlevel = 1,
                EntityType = "TextSpace.RoomEntities.ScrapDealerNpc",
                ActorFlavors = new RoomActorFlavor[] { RoomActorFlavor.Npc },
                RoomFlavors = new RoomFlavor[] { RoomFlavor.Empty, RoomFlavor.Nebula },
                Values = new Dictionary<string, string>
                {
                    { ValueKeys.Name, "Shady Scrap Dealer" },
                    { ValueKeys.DialogueText, $"The <> trade vessel looks real shoddy. Cables and spare parts are everywhere." },
                    { ValueKeys.LookText, "A <> looks a bit sketchy." }
                },
                Stats = new Dictionary<string, int> 
                {
                    { StatKeys.ScrapToResourcium, 10 }
                }
            }
        };

        public static List<FlexData> SometimesDamageHazards => new List<FlexData>
        {
            new FlexData()
            {
                ActorFlavors = new RoomActorFlavor[] { RoomActorFlavor.Hazard },
                RoomFlavors = new RoomFlavor[] { RoomFlavor.Empty, RoomFlavor.Nebula },
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
                ActorFlavors = new RoomActorFlavor[] { RoomActorFlavor.Hazard },
                RoomFlavors = new RoomFlavor[] { RoomFlavor.Kelp },
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
                ActorFlavors = new RoomActorFlavor[] { RoomActorFlavor.Hazard },
                RoomFlavors = new RoomFlavor[] { RoomFlavor.Empty },
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

        public static Dictionary<RoomFlavor, IEnumerable<string>> InjectableRoomLookTexts => new Dictionary<RoomFlavor, IEnumerable<string>>
        {
            { RoomFlavor.Empty, new string[] {
                "The sector of <> is oddly unsettling.",
                "The <> seems to go on forever into the stars.",
                "The immense void of <> is overwhelmingly dark."
            } },
            { RoomFlavor.Nebula, new string[] {
                "The purple <> clouds your view in every direction.",
                "The pulsating <> shifts onto itself all around you."
            } },
            { RoomFlavor.Kelp, new string[] {
                "Giant undulating space flora of a <> obstructs vision.",
                "A <> teems with life in the vaccum of space.",
                "Huge viridian stalks of a <> sway back and forth."
            } }
        };

        public static Dictionary<RoomFlavor, IEnumerable<string>> InjectableRoomDescriptions => new Dictionary<RoomFlavor, IEnumerable<string>>
        {
            { RoomFlavor.Empty, new string[] {
                "The empty void of space. Not much to see.",
                "The birthplace of stars turns out to be a great place to hide a burgeoning pirate base.",
                "The vast expanse of nothing-ness goes on in every direction. It is both inspiring and unsettling.",
                "A vast nothingness of space seems to stretch on forever. Systems pickup nearby signals, but it could hundreds of lightyears to the nearest civilian planet."
            } },
            { RoomFlavor.Nebula, new string[] {
                "Endless clouds of bursting energy form and dissipate as the Nebula writhes through space.",
                "An especially dense cloud makes sensor readings of any kind difficult"
            } },
            { RoomFlavor.Kelp, new string[] {
                "The massive plant life of the ancient Kelp Forest contains many dangers."
            } }
        };
    }
}