using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Stats;
using TextEncoding;
using System.Linq;
using EnumerableExtensions;
using Items;
using UnityEngine;

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
                        [StatKeys.PulsarCoreFragments] = 0,
                        [StatKeys.Credits] = 0,
                        [StatKeys.MaxHardwareSlots] = 2,
                        [StatKeys.Energy] = 10,
                        [StatKeys.MaxEnergy] = 10
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
                        [ValueKeys.LightWeapon] = PulseLaser.Key,
                        [ValueKeys.HeavyWeapon] = PlasmaTorpedo.Key
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

        public TagString GetLookText()
        {
            var texts = new List<string>
            {
                "<> jump into the sector.".Encode("You", Id, LinkColors.Player),
                "<> arrives in the sector".Encode("Your ship", Id, LinkColors.Player),
                "<> warp drive spins down.".Encode("Your", Id, LinkColors.Player),
                "<> drop out of warp.".Encode("You", Id, LinkColors.Player),
            };

            return new TagString()
            {
                Text = texts.GetRandom()
            };
        }
        
        public int MaxHull
        {
            get
            {
                return Stats[StatKeys.MaxHull];
            }

            set
            {
                Stats[StatKeys.MaxHull] = value;
            }
        }

        public IWeapon LightWeapon => WeaponFactory.GetWeapon(Values[ValueKeys.LightWeapon]);
        public IWeapon HeavyWeapon => WeaponFactory.GetWeapon(Values[ValueKeys.HeavyWeapon]);

        public string GetLinkText()
        {
            return "You";
        }

        public IRoomAction MainAction(IRoom room)
        {
            throw new NotImplementedException();
        }

        public void ChangeState(int nextState)
        {
            throw new NotImplementedException();
        }

        public void CalculateDialogue(IRoom room)
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
                passText = "Think of a plan";
            }

            DialogueContent = DialogueBuilder.Init()
                .AddMainText("Your ship looks like a standard frigate.")
                .AddTextA("Shields Up")
                .AddActionA(new CreateShieldActorAction(room.PlayerShip, room.PlayerShip, 3, 5))
                .AddTextB(passText)
                .AddActionB(passAction)
                .Build();
        }
        
        public CommandShip()
        {
            Id = Guid.NewGuid().ToString();
            
            //  temporary testing
            EquipHardware(new HazardDetector());
            EquipHardware(new MobDetector());
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

        public IRoomAction CleanupStep(IRoom room)
        {
            return new DoNothingAction(this);
        }

        private List<Hardware> _hardware = new List<Hardware>();
        public IEnumerable<Hardware> Hardware { get { return _hardware; } }

        public void EquipHardware(Hardware h)
        {
            if (_hardware.Count < Stats[StatKeys.MaxHardwareSlots])
            {
                _hardware.Add(h);
            }
        }

        public void DropHardware(Hardware h)
        {
            _hardware.Remove(h);            
        }

        public bool CheckHardware<T>() where T : Hardware
        {
            return Hardware.Any(h => h is T);
        }

        public T GetHardware<T>() where T : Hardware
        {
            return (T)Hardware.FirstOrDefault(h => h is T);
        }

        public static partial class ShipStats
        {
            public const string WarpDriveReady = "warp_drive_ready";
            public const string CaptainName = "captain_name";
        }
    }
}