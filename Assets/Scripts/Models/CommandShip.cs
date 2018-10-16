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
using Models.Dtos;
using Models.Factories;

namespace Models
{
    public class CommandShip : IRoomActor
    {
        public string Id { get; }
        public ABDialogueContent DialogueContent { get; set; }

        #region Stats
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

        public int Scrap
        {
            get
            {
                return Stats[StatKeys.Scrap];
            }

            set
            {
                Stats[StatKeys.Scrap] = value;
            }
        }

        public int Resourcium
        {
            get
            {
                return Stats[StatKeys.Resourcium];
            }

            set
            {
                Stats[StatKeys.Resourcium] = value;
            }
        }

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
                        [StatKeys.WarpDriveReady] = 0,
                        [StatKeys.Scrap] = 21,
                        [StatKeys.Resourcium] = 0,
                        [StatKeys.Techanite] = 0,
                        [StatKeys.MachineParts] = 0,
                        [StatKeys.PulsarCoreFragments] = 0,
                        [StatKeys.Credits] = 0,
                        [StatKeys.Energy] = 10,
                        [StatKeys.MaxEnergy] = 10,
                        [StatKeys.MaxHardwareSlots] = 5
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
                        [ValueKeys.CaptainName] = PickRandomCaptainName(),
                        [ValueKeys.LightWeapon] = LightWeapon.Name,
                        [ValueKeys.HeavyWeapon] = HeavyWeapon.Name
                    };
                }
                return _values;
            }
            private set
            {
                _values = value;
            }
        }
        #endregion

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
                return Stats[StatKeys.WarpDriveReady] != 0;
            }
            set
            {
                Stats[StatKeys.WarpDriveReady] = value ? 1 : 0;
            }
        }

        public RoomTemplate WarpTarget;

        public bool IsAttackable { get { return true; } set { throw new Exception("Tried to set CommandShip CanCombat to true"); } }

        public string DependentActorId { get; }

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
                //.AddActionA(new CreateShieldActorAction(room.PlayerShip, room.PlayerShip, 3, 5))
                .AddActionA(new GiveASpeechAction(this))
                .AddTextB(passText)
                .AddActionB(passAction)
                .Build();
        }
        
        public CommandShip()
        {
            Id = Guid.NewGuid().ToString();
            _hardware = new List<Hardware>();
        }

        public CommandShip(ShipDto dto)
        {
            Id = dto.Id;
            Stats = dto.Stats;
            Values = dto.Values;

            _hardware = new List<Hardware>();
            foreach (var h in dto.HardwareData)
            {
                _hardware.Add((Hardware)h.FromDto());
            }

            LightWeapon = (Weapon) dto.LightWeapon.FromDto();
            HeavyWeapon = (Weapon) dto.HeavyWeapon.FromDto();
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

        #region Weapons
        public Weapon LightWeapon { get; private set; }
        public Weapon HeavyWeapon { get; private set; }
        public Weapon SwapWeapon(Weapon weapon)
        {
            if (weapon.WeaponType == Weapon.WeaponTypes.Light)
            {
                var previous = LightWeapon;
                LightWeapon = weapon;
                return previous;
            }
            else
            {
                var previous = HeavyWeapon;
                HeavyWeapon = weapon;
                return previous;
            }
        }
        #endregion

        #region Hardware
        private List<Hardware> _hardware;
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
        #endregion
    }
}