using System;
using System.Collections.Generic;
using System.Linq;
using Models.Actions;
using Models.Dialogue;
using Models.Factories.Helpers;
using Repository;

namespace Models.Factories
{
    public sealed class RoomFactory : IRoomFactory
    {
        private readonly IEnumerable<IRoom> _rooms;

        public RoomFactory()
        {
        }

        public IRoom GenerateRoom(RoomTemplate template)
        {
            var randomRoomText = RoomRepository.ExampleRoomText.OrderBy(t => Guid.NewGuid()).First();
            //var room = new Room(randomRoomText[0], randomRoomText[1]);

            //TODO: need to figure out how to calculate the next templates
            var roomTemplate1 = new RoomTemplate(1, RoomFlavor.Kelp);
            var roomTemplate2 = new RoomTemplate(1, RoomFlavor.Kelp);
            var exits = new List<RoomTemplate>() { roomTemplate1, roomTemplate2 };
            
            foreach (var actor in GetActors(template))
            {
                //room.AddEntity(actor);
            }
            throw new NotImplementedException();
            //return room;
        }

        private IEnumerable<IRoomActor> GetActors(RoomTemplate template)
        {
            switch (template.Flavor)
            {
                case RoomFlavor.Kelp:
                    return RoomFactoryHelper.BuildKelpForestActors(template);
                case RoomFlavor.Nebula:
                    return RoomFactoryHelper.BuildKelpForestActors(template);
                case RoomFlavor.Empty:
                    return RoomFactoryHelper.BuildKelpForestActors(template);
            }
            
            throw new NotSupportedException();
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