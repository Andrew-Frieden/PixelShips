using System;
using Actions;
using Models.Text;
using PixelShips.Helpers;
using UnityEditor;

namespace Models
{
    public class Mob : IRoomEntity, ICombatEntity
    {
        public string Id { get; }
        public int Hull { get; set; }
        public string Description { get; }
        public string Link { get; }

        public Mob(string description, string link, int hull)
        {
            Id = Guid.NewGuid().ToString();
            Description = description;
            Link = link;
            Hull = hull;
        }

        public TextBlock GetLookText()
        {
            return new TextBlock(Description.GetDescriptionWithLink(Link, Id, "red"), Id);
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
            return new AttackAction(this, s.PlayerShip);
        }

        public ABDialogueContent GetInteraction(CmdState s)
        {
            throw new System.NotImplementedException();
        }
    }
}