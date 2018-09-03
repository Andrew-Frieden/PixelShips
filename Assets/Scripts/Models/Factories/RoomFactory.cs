using System;
using System.Collections.Generic;
using System.Linq;
using Repository;

namespace Models.Factories
{
    public class RoomFactory
    {
        private readonly IEnumerable<IRoom> _rooms;
        private readonly IEnumerable<IRoomActor> _actors;

        public RoomFactory()
        {
            var roomRepository = new RoomRepository();
            var entityRepository = new RoomEntityRepository();
            
            _rooms = roomRepository.LoadData();
            _actors = entityRepository.LoadData();
        }

        public IRoom GenerateRoom(RoomTemplate template, bool hasExits)
        {
            var roomTemplate1 = new RoomTemplate(1, RoomFlavor.Kelp, "gathering");
            var roomTemplate2 = new RoomTemplate(1, RoomFlavor.Kelp, "gathering");
            var roomTemplates = new List<RoomTemplate>() { roomTemplate1, roomTemplate2 };

            var randomizedRooms = _rooms.OrderBy(a => Guid.NewGuid()).ToList();
            var skeletonRoom = randomizedRooms.First();

            //TODO: Take away setter
            skeletonRoom.Description = RandomizeLeadIn() + skeletonRoom.Description;
            skeletonRoom.DialogueContent = new ABDialogueContent
            {
                MainText = "This is just placeholder text for the room dialogue content.",
                OptionAText = "Option A.",
                OptionBText = "Option B."
            };

            var tabby = _actors.First();
            skeletonRoom.AddEntity(tabby);

            //TODO: Take away setter
            if (hasExits)
            {
                skeletonRoom.Exits = new List<IRoom>()
                {
                    GenerateRoom(new RoomTemplate(1, RoomFlavor.Kelp, "trade"), false),
                    GenerateRoom(new RoomTemplate(1, RoomFlavor.Kelp, "trade"), false)
                };
            }

            return skeletonRoom;
        }

        private static string RandomizeLeadIn()
        {
            var leadInTexts = new List<string>
            {
                "Your jump leaves you in a < >. ",
                "Your ship arrives in a < >. ",
                "Your sensors indicate that the system you’ve entered is a < >. ",
                "Out the window you see a < >. ",
                "Your command viewport resolves into a view of a < >. ",
                "As your jump drives spin down, you look out to see a < >. ",
                "Jump drives still whirring, a < > fills your screen. ",
                "As your ship decelerates rapidly, a < > comes into view. ",
                "\"Captain!  We've spotted a nearby < >,\" your first mate shouts. ",
                "\"We have arrived at your destination... a < >... thank you for flying jump drive spacelines!\" "

            };

            return leadInTexts[UnityEngine.Random.Range(0, leadInTexts.Count)];
        }
    }
}