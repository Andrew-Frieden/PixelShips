using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Stats;
using TextEncoding;
using Links.Colors;
using Models.Dtos;

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
                    _stats = new Dictionary<string, int>
                    {
                        [ShipDto.StatKeys.IsAlive] = 1,
                        [StatKeys.MaxHull] = 20,
                        [StatKeys.Hull] = 1,
                        [StatKeys.MaxShields] = 20,
                        [StatKeys.Shields] = 20,
                        [StatKeys.Captainship] = 11,
                        [ShipDto.StatKeys.WarpDriveReady] = 0,
                        [StatKeys.Scrap] = 0,
                        [StatKeys.Resourcium] = 0,
                        [StatKeys.Techanite] = 0,
                        [StatKeys.MachineParts] = 0,
                        [StatKeys.PulsarCoreFragments] = 0
                    };
                }
                return _stats;
            }
            private set => _stats = value;
        }

        private Dictionary<string, string> _values;
        public Dictionary<string, string> Values
        {
            get
            {
                if (_values == null)
                {
                    _values = new Dictionary<string, string>()
                    {
                        [ShipDto.StatKeys.CaptainName] = PickRandomCaptainName(),
                    };
                }
                return _values;
            }
            private set => _values = value;
        }

        public bool Hidden { get; }

        public bool IsAggro
        {
            get => Stats[StatKeys.IsAggro] != 0;
            set => Stats[StatKeys.IsAggro] = value ? 1 : 0;
        }

        public bool WarpDriveReady
        {
            get => Stats[ShipDto.StatKeys.WarpDriveReady] != 0;
            set => Stats[ShipDto.StatKeys.WarpDriveReady] = value ? 1 : 0;
        }

        public RoomTemplate WarpTarget;

        public bool CanCombat => true;

        public string GetLookText()
        {
            return "< > jump into the sector.".Encode(GetLinkText(), Id, LinkColors.Player);
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
                    .AddActionA(new WarpAction(room.Exits[0]))
                    .AddTextB("Warp to room B.")
                    .AddActionB(new WarpAction(room.Exits[1]))
                    .Build();
            }
            
            return DialogueBuilder.Init()
                .AddMainText("Your ship looks like a standard frigate.")
                .AddTextA("Create a shield.")
                .AddActionA(new CreateShieldActorAction(room.PlayerShip, room.PlayerShip, 3, 5))
                .AddTextB("Spin up your warp drive.")
                .AddActionB(new CreateWarpDriveActorAction(1))
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

        private string PickRandomCaptainName()
        {
            var random = UnityEngine.Random.Range(0, 5);

            switch (random)
            {
                case 0:
                    return "Space Pirate Dave";
                case 1:
                    return "Space Bro Chad";
                case 2:
                    return "Captain P.W. Underpants";
                case 3:
                    return "John 'deep space' robinson";
                case 4:
                default:
                    return "The dread pirate spigs";
            }
        }
    }
}