using System.Collections.Generic;

namespace GameData
{
    public static class ExampleGameData
    {
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
            } },
            { RoomFlavor.Rural, new string[] {
                "Soothing vistas of <> stretch beyond vision.",
                "This bucolic and serene system must be <>.",
                "Hardworking Agrinauts bustle back and forth through the <>."
            } },
            { RoomFlavor.Grid, new string[] {
                "The <>, a digital frontier to reshape -null- existance.",
                "The Master Control Program has chosen you to visit the <>.",
                "In the <>, ships compete for the benefit of the -null-."
            } },
            { RoomFlavor.Solar, new string[] {
                "A bright star shines upon the vast cat empire of the <>.",
                "Golden pyrammids and obelisks make the <> system glitter.",
                "Within the <> space itself seems to take on a golden aura."
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
            } },
            { RoomFlavor.Rural, new string[] {
                "The deceptive calm of the Agrinaut homeland has lulled many into a false sense of security."
            } },
            { RoomFlavor.Grid, new string[] {
                "A digital frontier stretches beyond view, what unknown treaures and dangers wait within the -XXXXXXXX------ SEG FAULT>>>..."
            } },
            { RoomFlavor.Solar, new string[] {
                "A core world of the Noble Cat Alliance, monumental golden pyramids and obelisks can be seen clearly even from this distance.",
                "A military installation of the Noble Cat Alliance, the architect was clearly more excited in plating things in gold than adding more defenses.",
                "Busy trading ships sail in and out while patrol vessels scan furiously for contraband such as techanite or nip."
            } }
        };
    }
}