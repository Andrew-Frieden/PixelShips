using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using System;
using System.Collections.Generic;
using TextEncoding;

namespace Items
{
    public class Hardware : FlexEntity
    {
        public enum HardwareState
        {
            Unequipped = 0,
            Equipped = 1
        }

        public override void CalculateDialogue(IRoom room)
        {
            switch (CurrentState)
            {
                case (int)HardwareState.Unequipped:

                    var builder = DialogueBuilder.Init()
                        .AddMainText($"<>{Env.ll}{DialogueText}{Env.ll}This hardware is drifting through space.".Encode(this, LinkColors.Gatherable));

                    if (room.PlayerShip.OpenHardwareSlots > 0)
                        builder.AddOption("Pickup", new PickupHardwareAction(room.PlayerShip, this));

                    DialogueContent = builder.Build(room);
                    break;
                case (int)HardwareState.Equipped:
                    DialogueContent = DialogueBuilder.Init()
                        .AddMainText($"<>{Env.ll}{DialogueText}{Env.ll}Currently equipped to your ship.".Encode(this, LinkColors.Gatherable))
                        .AddOption("Drop", new DropHardwareAction(room.PlayerShip, this))
                        .Build(room);
                    break;
            }
        }

        public override TagString GetLookText()
        {
            return "A <> drifts through space.".Encode(this, LinkColors.Gatherable).Tag();
        }

        public override IRoomAction MainAction(IRoom room)
        {
            return new DoNothingAction(this);
        }
        
        public Hardware(FlexData data) : base(data)
        {
            IsHostile = false;
            IsAttackable = false;
        }

        public Hardware(FlexEntityDto dto) : base(dto) { }

        public Hardware() : base()
        {
            IsAttackable = false;
            IsHostile = false;
            ChangeState((int)HardwareState.Equipped);
        }

        public override void ChangeState(int nextState)
        {
            base.ChangeState(nextState);

            if (nextState == (int)HardwareState.Unequipped)
            {
                IsHidden = true;
            }
            else if (nextState == (int)HardwareState.Unequipped)
            {
                IsHidden = false;
            }
        }
    }

    public class TownDetector : Hardware
    {
        public TownDetector() : base()
        {
            Name = "Cartographic Processor";
            DialogueText = "Quantum mapping device that identifies nearby space stations.";
        }

        public TownDetector(FlexEntityDto dto) : base(dto) { } 
        public TownDetector(FlexData data) : base(data) { } 
    }

    public class HazardDetector : Hardware
    {
        public HazardDetector() : base()
        {
            Name = "Deep Field Scanner";
            DialogueText = "A sophisticated navigation device capable of detecting nearby hazards.";
        }

        public HazardDetector(FlexEntityDto dto) : base(dto) { }
        public HazardDetector(FlexData data) : base(data) { } 
    }

    public class SuperDetector : Hardware
    {
        public SuperDetector(FlexData data) : base(data) { }
        public SuperDetector(FlexEntityDto dto) : base(dto) { }
    }

    public class MobDetector : Hardware
    {
        public MobDetector() : base()
        {
            Name = "Signature Detector";
            DialogueText = "Standard-issue military hardware for detecting nearby weapon signatures.";
        }

        public MobDetector(FlexEntityDto dto) : base(dto) { }
        public MobDetector(FlexData data) : base(data) { } 
    }

    public class GatheringBoost : Hardware
    {
        public GatheringBoost() : base()
        {
            Name = "Robotic Manipulator";
            DialogueText = "An advanced gathering tool capable of crushing rock.";
        }

        public GatheringBoost(FlexEntityDto dto) : base(dto) { }
        public GatheringBoost(FlexData data) : base(data) { } 

        private const float boost = 0.5f;

        public void ApplyBoost(ref int amount)
        {
            amount += (int)(amount * boost);
        }
    }

    public class HazardMitigation : Hardware
    {
        public HazardMitigation() : base()
        {
            Name = "Hazard Plating";
            DialogueText = "Advanced hull polarization that provides significant resistance to the hazards of space.";
        }

        public HazardMitigation(FlexEntityDto dto) : base(dto) { }
        public HazardMitigation(FlexData data) : base(data) { }
    }
    
    public class MaxHullPlating : Hardware
    {
        public const string MaxHullBonusKey = "max_hull_bonus";
        public int MaxHullBonus
        {
            get
            {
                return Stats[MaxHullBonusKey];
            }
            protected set
            {
                Stats[MaxHullBonusKey] = value;
            }
        }
        
        public MaxHullPlating() : base()
        {
            Name = "Armor Plating";
            DialogueText = "Hull plating that provides additional durability.";
            MaxHullBonus = 10;
        }
        
        public MaxHullPlating(FlexEntityDto dto) : base(dto) { }
        public MaxHullPlating(FlexData data) : base(data) { }
    }

    public class DropHardwareAction : SimpleAction
    {
        public DropHardwareAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }

        public DropHardwareAction(IRoomActor src, IRoomActor target)
        {
            if (target is Hardware)
                Target = target;
            else
                throw new InvalidOperationException();

            Source = src;
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            var hardware = (Hardware)Target;

            if (Source == room.PlayerShip)
                room.PlayerShip.DropHardware(hardware);

            hardware.IsDestroyed = false;
            room.AddEntity(hardware);
            hardware.ChangeState((int)Hardware.HardwareState.Unequipped);
            //PrimaryTripleViewController.ShowPrimary((int)TripleView.Middle);

            return $"<> is dropped into space.".Encode(Target, LinkColors.Gatherable).ToTagSet(new[] { UIResponseTag.ViewCmd, UIResponseTag.HideNavBar });
        }
    }

    public class PickupHardwareAction : SimpleAction
    {
        public PickupHardwareAction(SimpleActionDto dto, IRoom room) : base(dto, room) { }

        public PickupHardwareAction(IRoomActor src, IRoomActor target)
        {
            if (target is Hardware)
                Target = target;
            else
                throw new InvalidOperationException();

            Source = src;
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            var hardware = (Hardware)Target;

            if (Source == room.PlayerShip)
                room.PlayerShip.EquipHardware(hardware);

            //hardware.IsDestroyed = true;
            //hardware.ChangeState((int)Hardware.HardwareState.Equipped);

            return $"You pickup the <>".Encode(Target, LinkColors.Gatherable).ToTagSet(new[] { UIResponseTag.ShowNavBar });
        }
    }
}