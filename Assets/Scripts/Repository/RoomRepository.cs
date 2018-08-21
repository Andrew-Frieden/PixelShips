using System.Collections.Generic;
using Models;

namespace Repository
{
    public class RoomRepository : IJsonRepository<IRoom>
    {
        public IEnumerable<IRoom> LoadData()
        {
            var rooms = new List<IRoom>();

            var giantNebulaContent = new ABDialogueContent()
            {
                MainText = @"This sector seems to be a dizzying array of star dust and deadly asteroids.",
                OptionAText = "Warp to next sector",
                OptionBText = "This should just be an A or cancel I think"
            };

            rooms.Add(new Room("You enter into a {{ link }} with many asteroids.", "Giant Nebula", null, new List<IRoomEntity>() { }, giantNebulaContent));



            rooms.Add(generateRoom(randomizeLeadIn(), "Space Kelp Forest",
                "Giant undulating spaceborne plant strands obstruct your view of the galaxies beyond.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Green Star Periphery",
                "Not a star in as such, but a glowing mass of writhing plant matter.  The light it gives off is eerie.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Goldilocks Zone",
                "Not too hot, not too cold, the perfect place for life to thrive out in space.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Citadel of Lions",
                "A core world of the Noble Cat Alliance, monumental golden pyramids and obelisks can be seen clearly even from this distance.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Jaguar-Class Outpost",
                "A military installation of the Noble Cat Alliance, the architect was clearly more excited in plating things in gold than adding more defenses.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Siamese Trade Port",
                "Busy trading ships sail in and out while patrol vessels scan furiously for contraband such as techanite or nip.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Bromicron of Olympia",
                "A sacred temple for the Fraternitus Barbarinus.  Both ancient and collegiate, their keggers are legendary.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Weapon Smithery of Vulkania",
                "Every barbarian worth his letters needs a signature cannon, torpedo, harpoon, or laser to strike fear into his enemies.  This is where they’re forged.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Deep Space Salvage",
                "Fields of proto-sentient debris drift in every direction.  Their cycle of formation, consumption, and deconstruction is somehow very familiar.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Nebula Tropics",
                "The birthplace of stars turns out to be a great place to hide a burgeoning pirate base.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Flotilla Cantina",
                "What was once a tenuously cordoned meeting place for pirate ships is now a tight-knit, established watering hole.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Blackwater Caye",
                "Every pirate wants to leave a legacy of buried treasures, the planets in this sector are literally covered in Xs of every description.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Zone of Empty Space",
                "The vast expanse of nothing-ness goes on in every direction.  It is both inspiring and unsettling.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Pulsar’s Spallation Field",
                "The frighteningly hot neutron star spins rapidly, spitting out hot radiation at a rather up-beat tempo.", "Warp Left", "Warp Right"));

            rooms.Add(generateRoom(randomizeLeadIn(), "Cloud of Asteroids",
                "Floating rocks small and large block your otherwise pristine view.", "Warp Left", "Warp Right"));



            //TODO: Remove commandship and entities from constructor and add them post content creation so the repository isn't dependant on game state

            return rooms;
        }

        private Room generateRoom(string leadIn, string roomName, string mainText, string optionAText, string optionBText)
        {
            var roomContent = new ABDialogueContent()
            {
                MainText = mainText,
                OptionAText = optionAText,
                OptionBText = optionBText

            };

            return new Room( leadIn + mainText, roomName, null, new List<IRoomEntity>() { }, roomContent);
        }

        private string randomizeLeadIn()
        {
            var leadInTexts = new List<string>
            {
                "Your jump leaves you in a {{ link }}. ",
                "Your ship arrives in a {{ link }}. ",
                "Your sensors indicate that the system you’ve entered is a {{ link }}. ",
                "Out the window you see a {{ link }}. ",
                "Your command viewport resolves into a view of a {{ link }}. ",
                "As your jump drives spin down, you look out to see a {{ link }}. ",
                "Jump drives still whirring, a {{ link }} fills your screen. ",
                "As your ship decelerates rapidly, a {{ link }} comes into view. ",
                "\"Captain!  We've spotted a nearby {{ link }},\" your first mate shouts. ",
                "\" We have arrived at your destination... a {{ link }}... thank you for flying jump drive spacelines!\" "

            };

            return leadInTexts[UnityEngine.Random.Range(0, leadInTexts.Count)];
        }
    }
}