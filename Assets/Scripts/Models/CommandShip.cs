using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Stats;
using TextEncoding;
using Links.Colors;
using System.Linq;

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
                        [ShipStats.IsAlive] = 1,
                        [StatKeys.MaxHull] = 20,
                        [StatKeys.Hull] = 20,
                        [StatKeys.MaxShields] = 20,
                        [StatKeys.Shields] = 20,
                        [StatKeys.Captainship] = 11,
                        [ShipStats.WarpDriveReady] = 0,
                        [StatKeys.Scrap] = 0,
                        [StatKeys.Resourcium] = 0,
                        [StatKeys.Techanite] = 0,
                        [StatKeys.MachineParts] = 0,
                        [StatKeys.PulsarCoreFragments] = 0
                    };
                }
                return _stats;
            }
            private set
            {
                _stats = value;
            }
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
                        [ShipStats.CaptainName] = PickRandomCaptainName(),
                    };
                }
                return _values;
            }
            private set
            {
                _values = value;
            }
        }

        public bool IsHidden { get; }

        public bool IsHostile
        {
            get
            {
                if (!Stats.ContainsKey(StatKeys.IsHostile))
                    Stats[StatKeys.IsHostile] = 0;
                return Stats[StatKeys.IsHostile] != 0;
            }
            set
            {
                Stats[StatKeys.IsHostile] = value ? 1 : 0;
            }
        }

        public bool WarpDriveReady
        {
            get
            {
                return Stats[ShipStats.WarpDriveReady] != 0;
            }
            set
            {
                Stats[ShipStats.WarpDriveReady] = value ? 1 : 0;
            }
        }

        public RoomTemplate WarpTarget;

        public bool IsAttackable { get { return true; } set { throw new Exception("Tried to set CommandShip CanCombat to true"); } }

        public bool IsDestroyed
        {
            get
            {
                return Stats[StatKeys.Hull] <= 0;
            }
            set
            {
                if (value)
                    Stats[StatKeys.Hull] = 0;
            }
        }

        public int Hull
        {
            get
            {
                return Stats[StatKeys.Hull];
            }

            set
            {
                Stats[StatKeys.Hull] = value;
            }
        }

        public StringTagContainer GetLookText()
        {
            return new StringTagContainer()
            {
                Text = "< > jump into the sector.".Encode(GetLinkText(), Id, LinkColors.Player)
            };
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
            IRoomAction passAction;
            string passText;

            if (room.Entities.Any(ent => ent.IsHostile))
            {
                passAction = new BarrellRollAction(this);
                passText = "Evasive Maneuvers";
            }
            else
            {
                passAction = new GiveASpeechAction(this);
                passText = "Give a Speech";
            }

            return DialogueBuilder.Init()
                .AddMainText("Your ship looks like a standard frigate.")
                .AddTextA("Overcharge Shields")
                .AddActionA(new CreateShieldActorAction(room.PlayerShip, room.PlayerShip, 3, 5))
                .AddTextB(passText)
                .AddActionB(passAction)
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
                    return "John 'deep space' Robinson";
                default:
                    return "The dread pirate spigs";
            }
        }

        public static partial class ShipStats
        {
            public const string IsAlive = "is_alive";

            public const string WarpDriveReady = "warp_drive_ready";
            public const string CaptainName = "captain_name";
        }
    }
}