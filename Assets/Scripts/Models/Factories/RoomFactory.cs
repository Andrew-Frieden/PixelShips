using System.Collections.Generic;

namespace Models.Factories
{
    public static class RoomFactory
    {
        public static Room GenerateRoom(string roomName, string mainText, string optionAText, string optionBText)
        {
            var roomContent = new ABDialogueContent()
            {
                MainText = mainText,
                OptionAText = optionAText,
                OptionBText = optionBText

            };

            return new Room(RandomizeLeadIn() + mainText, roomName, null, new List<IRoomActor>() { }, roomContent);
        }

        private static string RandomizeLeadIn()
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
                "\"We have arrived at your destination... a {{ link }}... thank you for flying jump drive spacelines!\" "

            };

            return leadInTexts[UnityEngine.Random.Range(0, leadInTexts.Count)];
        }
    }
}