using System.Collections.Generic;
using Models;
using Models.Factories;

namespace Repository
{
    public class RoomRepository : IJsonRepository<IRoom>
    {
        public IEnumerable<IRoom> LoadData()
        {
            var rooms = new List<IRoom>
            {
                new Room("Space Kelp Forest",
                    "Giant undulating spaceborne plant strands obstruct your view of the galaxies beyond."),
                new Room("Green Star Periphery",
                    "Not a star in as such, but a glowing mass of writhing plant matter.  The light it gives off is eerie."),
                new Room("Goldilocks Zone",
                    "Not too hot, not too cold, the perfect place for life to thrive out in space."),
                new Room("Citadel of Lions",
                    "A core world of the Noble Cat Alliance, monumental golden pyramids and obelisks can be seen clearly even from this distance."),
                new Room("Jaguar-Class Outpost",
                    "A military installation of the Noble Cat Alliance, the architect was clearly more excited in plating things in gold than adding more defenses."),
                new Room("Siamese Trade Port",
                    "Busy trading ships sail in and out while patrol vessels scan furiously for contraband such as techanite or nip."),
                new Room("Bromicron of Olympia",
                    "A sacred temple for the Fraternitus Barbarinus.  Both ancient and collegiate, their keggers are legendary."),
                new Room("Weapon Smithery of Vulkania",
                    "Every barbarian worth his letters needs a signature cannon, torpedo, harpoon, or laser to strike fear into his enemies.  This is where they’re forged."),
                new Room("Deep Space Salvage",
                    "Fields of proto-sentient debris drift in every direction.  Their cycle of formation, consumption, and deconstruction is somehow very familiar."),
                new Room("Nebula Tropics",
                    "The birthplace of stars turns out to be a great place to hide a burgeoning pirate base."),
                new Room("Flotilla Cantina",
                    "What was once a tenuously cordoned meeting place for pirate ships is now a tight-knit, established watering hole."),
                new Room("Blackwater Caye",
                    "Every pirate wants to leave a legacy of buried treasures, the planets in this sector are literally covered in Xs of every description."),
                new Room("Zone of Empty Space",
                    "The vast expanse of nothing-ness goes on in every direction.  It is both inspiring and unsettling."),
                new Room("Pulsar's Spallation Field",
                    "The frighteningly hot neutron star spins rapidly, spitting out hot radiation at a rather up-beat tempo."),
                new Room("Cloud of Asteroids",
                    "Floating rocks small and large block your otherwise pristine view.")
            };

            return rooms;
        }
    }
}