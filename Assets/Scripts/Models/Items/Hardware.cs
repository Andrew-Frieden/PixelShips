using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
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
                    DialogueContent = DialogueBuilder.Init()
                        .AddMainText($"<>{Env.ll}{DialogueText}{Env.ll}This hardware is drifting through space.".Encode(this, LinkColors.Gatherable))
                        .AddOption("Pickup", new PickupHardwareAction(room.PlayerShip, this))
                        .Build();
                    break;
                case (int)HardwareState.Equipped:
                    DialogueContent = DialogueBuilder.Init()
                        .AddMainText($"<>{Env.ll}{DialogueText}{Env.ll}Currently equipped to your ship.".Encode(this, LinkColors.Gatherable))
                        .AddOption("Drop", new DropHardwareAction(room.PlayerShip, this))
                        .Build();
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

        public Hardware(FlexEntityDto dto) : base(dto)
        {
        }

        public Hardware(bool equipped) : base()
        {
            IsAttackable = false;
            IsHostile = false;

            if (equipped)
                ChangeState((int)HardwareState.Equipped);
            else
                ChangeState((int)HardwareState.Unequipped);
        }

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

    public class EmptyHardware
    {
        public const string Key = "empty_hardware";
        public string Name => "Empty Hardware";
    }

    public class TownDetector : Hardware
    {
        //public const string Key = "town_detector";

        public TownDetector() : base()
        {
            Name = "Cartographic Processor";
            DialogueText = "Quantum mapping device that identifies nearby space stations.";
        }

        public TownDetector(FlexEntityDto dto) : base(dto) { } 
    }

    public class HazardDetector : Hardware
    {
        //public const string Key = "hazard_detector";

        public HazardDetector() : base()
        {
            Name = "Deep Field Scanner";
            DialogueText = "A sophisticated navigation device capable of detecting nearby hazards.";
        }

        public HazardDetector(FlexEntityDto dto) : base(dto) { }
    }

    public class MobDetector : Hardware
    {
        //public const string Key = "mob_detector";

        public MobDetector() : base()
        {
            Name = "Signature Detector";
            DialogueText = "Standard-issue military hardware for detecting nearby weapon signatures.";
        }

        public MobDetector(FlexEntityDto dto) : base(dto) { }
    }

    public class GatheringBoost : Hardware
    {
        //public const string Key = "gathering_boost";

        public GatheringBoost() : base()
        {
            Name = "Robotic Manipulator";
            DialogueText = "An advanced gathering tool capable of crushing rock.";
        }

        public GatheringBoost(FlexEntityDto dto) : base(dto) { }

        private const float boost = 0.5f;

        public void ApplyBoost(ref int amount)
        {
            amount += (int)(amount * boost);
        }
    }

    public class HazardMitigation : Hardware
    {
        //public const string Key = "hazard_mitigation";

        public HazardMitigation() : base()
        {
            Name = "Hazard Plating";
            DialogueText = "Advanced hull reinforcements that provide significant resistance to the hazards of space.";
        }

        public HazardMitigation(FlexEntityDto dto) : base(dto) { }
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
            PrimaryTripleViewController.ShowPrimary((int)TripleView.Middle);

            return $"<> is dropped into space.".Encode(Target, LinkColors.Gatherable).ToTagSet();
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

            hardware.IsDestroyed = true;
            hardware.ChangeState((int)Hardware.HardwareState.Equipped);

            return $"You pickup the <>".Encode(Target, LinkColors.Gatherable).ToTagSet();
        }
    }
}