using System;
using System.Collections.Generic;
using Models.Actions;
using TextEncoding;

namespace Models
{
    public class Mob : IRoomActor, ICombatEntity
    {
        public string Id { get; }
        public int Hull { get; set; }
        public string Description { get; }
        public string Link { get; }
        public Dictionary<string, int> Stats { get; }
        public ABDialogueContent DialogueContent { get; set; }

        public bool IsHidden { get; set; }

        public bool IsHostile
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool CanCombat => true;

        public Dictionary<string, string> Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool IRoomActor.IsAttackable
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Mob(string description, string link, int hull, ABDialogueContent dialogueContent)
        {
            Id = Guid.NewGuid().ToString();
            Description = description;
            Link = link;
            Hull = hull;
            DialogueContent = dialogueContent;
        }

        public StringTagContainer GetLookText()
        {
            return null;
        }

        public string GetLinkText()
        {
            return Link;
        }

        public IRoomAction GetNextAction(IRoom s)
        {
            return new AttackAction(this, s.PlayerShip, 5, "Mob Weapon");
        }

        public void AfterAction(IRoom room) { }

        public void ChangeState(int nextState)
        {
            throw new NotImplementedException();
        }

        public ABDialogueContent CalculateDialogue(IRoom room)
        {
            throw new NotImplementedException();
        }
    }
}