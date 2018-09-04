using System;
using System.Collections.Generic;
using Models.Actions;
using TextEncoding;

namespace Models
{
    public class CommandShip : Ship, IRoomActor
    {
        public string Id { get; }
        public ABDialogueContent DialogueContent { get; set; }

        private Dictionary<string, int> _stats;
        public Dictionary<string, int> Stats
        {
            get
            {
                if (_stats == null)
                {
                    _stats = new Dictionary<string, int>();
                    _stats[StatKeys.Hull] = 10;
                    _stats[StatKeys.Captainship] = 11;
                    _stats[StatKeys.Resourcium] = 1;
                }
                return _stats;
            }
            private set { _stats = value; }
        }

        public bool IsAggro
        {
            get
            {
                return Stats[StatKeys.IsAggro] != 0;
            }
            set
            {
                Stats[StatKeys.IsAggro] = value ? 1 : 0;
            }
        }

        public bool CanCombat { get { return true; } }

        public string GetLookText()
        {
            return "< > jump into the sector.".Encode(GetLinkText(), Id, "blue");
        }

        public string GetLinkText()
        {
            return "You";
        }

        public IRoomAction GetNextAction(IRoom room)
        {
            throw new NotImplementedException();
        }

        public void AfterAction(IRoom room)
        {
            throw new NotImplementedException();
        }

        public void ChangeState(int nextState)
        {
            throw new NotImplementedException();
        }

        public ABDialogueContent CalculateDialogue(IRoom room)
        {
            throw new NotImplementedException();
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