using System;
using System.Collections.Generic;
using TextSpace.Models.Actions;
using TextSpace.Models.Dialogue;
using TextSpace.Models.Stats;
using TextEncoding;
using System.Linq;
using EnumerableExtensions;
using TextSpace.Items;
using TextSpace.Models.Dtos;
using TextSpace.Events;
using TextSpace.Services.Factories;

namespace TextSpace.Models
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
                var maxHull = Stats[StatKeys.MaxHull];

                var maxHullHardware = GetHardware<MaxHullPlating>();
                foreach (var hw in maxHullHardware)
                    maxHull += hw.MaxHullBonus;

                return maxHull;
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
                        [StatKeys.Resourcium] = 3,
                        [StatKeys.Techanite] = 0,
                        [StatKeys.MachineParts] = 0,
                        [StatKeys.PulsarCoreFragments] = 0,
                        [StatKeys.Credits] = 0,
                        [StatKeys.Energy] = 10,
                        [StatKeys.MaxEnergy] = 10,
                        [StatKeys.MaxHardwareSlots] = 3
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

        public bool IsHidden { get; set; }

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
                .Build(room);
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
            var names = new List<string>
            {
                "Daymar Donnall",
                "Blorn Terrend",
                "Kuna Melne",
                "Tonis Kara",
                "Jado Vikar",
                "Cen Carden",
                "Maro Grenko",
                "Garo Janren",
                "Haro Syko",
                "Orron Gravanc",
                "Daro Lahkar",
                "Gil Dellian",
                "Tenkin Forsey",
                "Cameron Molasky",
                "Drake Montegue",
                "Darius Malvo",
                "Nolan Nkuna",
                "Tomas Araujo",
                "Austin Troyer",
                "Jonathan Jakkerson",
                "Colo Hedsard",
                "Lon Baize",
                "Gavenk Alen",
                "Kenth Haren",
                "Maxir Typhe",
                "Luca Alon",
                "Vance Lockley",
                "Rory Solari",
                "Stanton Bowdoin",
                "Monty Breton",
                "Dorsey Warwick",
                "Jarred Desai",
                "Chas Alcantar",
                "Kennith Severt",
                "Herschel Sonoda",
                "Darell Tian"
            };

            return names[UnityEngine.Random.Range(0, names.Count)];
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
            Weapon previous;

            if (weapon.WeaponType == Weapon.WeaponTypes.Light)
            {
                previous = LightWeapon;
                LightWeapon = weapon;
            }
            else
            {
                previous = HeavyWeapon;
                HeavyWeapon = weapon;
            }

            weapon.ChangeState((int)Weapon.WeaponState.Equipped);

            if (previous != null)
                previous.ChangeState((int)Weapon.WeaponState.Unequipped);

            return previous;
        }
        #endregion

        #region Hardware
        private List<Hardware> _hardware;
        public IEnumerable<Hardware> Hardware { get { return _hardware; } }

        public int OpenHardwareSlots
        {
            get
            {
                return Stats[StatKeys.MaxHardwareSlots] - _hardware.Count;
            }
        }

        public string DependentActorId { get; set; }

        public void EquipHardware(Hardware h)
        {
            if (OpenHardwareSlots > 0)
            {
                _hardware.Add(h);

                h.IsDestroyed = true;
                h.ChangeState((int)Items.Hardware.HardwareState.Equipped);

                if (h is MaxHullPlating)
                    UIResponseBroadcaster.Broadcast(UIResponseTag.PlayerHullModified);
            }
        }

        public void DropHardware(Hardware h)
        {
            _hardware.Remove(h);

            if (h is MaxHullPlating)
            {
                if (Hull > MaxHull)
                    Hull = MaxHull;

                UIResponseBroadcaster.Broadcast(UIResponseTag.PlayerHullModified);
            }
        }

        public bool CheckHardware<T>() where T : Hardware
        {
            return Hardware.Any(h => h is T);
        }

        public IEnumerable<T> GetHardware<T>() where T : Hardware
        {
            return Hardware.Where(h => h is T).Select(i => (T)i);
        }
        #endregion
    }
}