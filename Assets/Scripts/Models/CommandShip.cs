using System;
using PixelShips.Helpers;

namespace Models
{
    public class CommandShip : Ship, ICombatEntity
    {
        public string Id { get; }
        public ABDialogueContent DialogueContent { get; }

        public string GetLookText()
        {
            return "You".GetLink("orange", Id);;
        }

        public string GetLinkText()
        {
            return "You".GetLink("orange", Id);
        }

        public CommandShip(int gathing, int transport, int intelligence, int combat, int speed, int hull) : base(gathing, transport, intelligence, combat, speed, hull)
        {
            Id = Guid.NewGuid().ToString();
            DialogueContent = new ABDialogueContent();
        }

        public CommandShip() { }
    }
}