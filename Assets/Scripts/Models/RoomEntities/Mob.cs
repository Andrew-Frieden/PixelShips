using System;
using Models.Actions;
using PixelShips.Helpers;

namespace Models
{
    public class Mob : IRoomActor, ICombatEntity
    {
        public string Id { get; }
        public int Hull { get; set; }
        public string Description { get; }
        public string Link { get; }
        public ABDialogueContent DialogueContent { get; set; }

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
            return Link;
        }

        public IRoomAction GetNextAction(IRoom s)
        {
            return new AttackAction(this, s.PlayerShip, 5);
        }
    }
}