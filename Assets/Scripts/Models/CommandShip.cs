using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using TextEncoding;

namespace Models
{
    public class CommandShip : IRoomActor
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
                    _stats[StatKeys.WarpDriveReady] = 0;
                }
                return _stats;
            }
            private set { _stats = value; }
        }

        public bool PrintToScreen { get; set; }

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

        public bool WarpDriveReady
        {
            get
            {
                return Stats[StatKeys.WarpDriveReady] != 0;
            }
            set
            {
                Stats[StatKeys.WarpDriveReady] = value ? 1 : 0;
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
            if (WarpDriveReady)
            {
                return DialogueBuilder.Init()
                    .AddMainText("Your ship looks like its ready to jump to hyperspace.")
                    .AddTextA("Warp to room A.")
                    .AddActionA(new WarpAction(room.RoomTemplates[0]))
                    .AddTextB("Warp to room B.")
                    .AddActionB(new WarpAction(room.RoomTemplates[1]))
                    .Build();
            }
            
            return DialogueBuilder.Init()
                .AddMainText("Your ship looks like a standard frigate.")
                .AddTextA("Create a shield.")
                .AddActionA(new CreateShieldActorAction(room.PlayerShip, null, 3, 5))
                .AddTextB("Spin up your warp drive.")
                .AddActionB(new CreateWarpDriveActorAction(2))
                .Build();
        }

        public CommandShip(int gathering, int transport, int intelligence, int combat, int speed, int hull)
        {
            Id = Guid.NewGuid().ToString();
            DialogueContent = new ABDialogueContent();
        }

        public CommandShip(string id, int gathering, int transport, int intelligence, int combat, int speed, int hull)
        {
            Id = id;
            DialogueContent = null;
        }
        
        public CommandShip()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}