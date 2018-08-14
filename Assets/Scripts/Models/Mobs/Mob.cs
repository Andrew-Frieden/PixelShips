using System;
using Actions;
using UnityEditor;

namespace Models
{
    public class Mob : IRoomEntity, ICombatEntity
    {
        public string Id { get; }
        public int Hull { get; set; }
        public string Description { get; }

        public Mob(string description, int hull)
        {
            Id = Guid.NewGuid().ToString();
            Description = description;
            Hull = hull;
        }

        public string GetLookText()
        {
            throw new System.NotImplementedException();
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