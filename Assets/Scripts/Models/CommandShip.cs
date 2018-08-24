using System;

namespace Models
{
    public class CommandShip : Ship, ICombatEntity, ITextEntity
    {
        public string Id { get; }
        public ABDialogueContent DialogueContent { get; set; }

        public string GetLookText()
        {
            return "[You] warped into the sector.";
        }

        public string GetLinkText()
        {
            return "link text";
            //throw new System.NotImplementedException();
        }

        public CommandShip(int gathering, int transport, int intelligence, int combat, int speed, int hull) : base(gathering, transport, intelligence, combat, speed, hull)
        {
            Id = Guid.NewGuid().ToString();
            DialogueContent = new ABDialogueContent();
        }

        public CommandShip(string id, int gathering, int transport, int intelligence, int combat, int speed, int hull) : base(gathering, transport, intelligence, combat, speed, hull)
        {
            Id = id;
            DialogueContent = null;
        }
    }
}