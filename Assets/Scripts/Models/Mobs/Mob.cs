using System;
using Actions;
using PixelShips.Helpers;

namespace Models
{
    public class Mob : IRoomEntity, ICombatEntity
    {
        public string Id { get; }
        public int Hull { get; set; }
        public string Description { get; }
        public string Link { get; }
        public ABDialogueContent DialogueContent { get; }

        public Mob(string description, string link, int hull, ABDialogueContent dialogueContent)
        {
            Id = Guid.NewGuid().ToString();
            Description = description;
            Link = link;
            Hull = hull;
            DialogueContent = dialogueContent;
        }

        public string GetLookText()
        {
            return Description.GetDescriptionWithLink(Link, Id, "red");
        }

        public string GetLinkText()
        {
            throw new System.NotImplementedException();
        }

        public ABDialogueContent GetInteraction(IRoom s)
        {
            throw new System.NotImplementedException();
        }

        public IRoomAction GetNextAction(IRoom s)
        {
            return new AttackAction(this, s.PlayerShip, 5);
        }

        public ABDialogueContent GetInteraction(CmdState s)
        {
            throw new System.NotImplementedException();
        }
    }
}