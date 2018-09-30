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
    //public static class HardwareFactory
    //{
    //    public static Hardware GetHardware(string key)
    //    {
    //        switch (key)
    //        {
    //            case TownDetector.Key:
    //                return new TownDetector();
    //            case HazardDetector.Key:
    //                return new HazardDetector();
    //            case MobDetector.Key:
    //                return new MobDetector();
    //            case GatheringBoost.Key:
    //                return new GatheringBoost();
    //            case HazardMitigation.Key:
    //                return new HazardMitigation();
    //        }

    //        throw new NotImplementedException();
    //    }
    //}

    public class Hardware : FlexEntity
    {
        public enum HardwareState
        {
            Unequipped = 0,
            Equipped = 1
        }

        public string Description
        {
            get
            {
                return Values[ValueKeys.DialogueText];
            }
            protected set
            {
                Values[ValueKeys.DialogueText] = value;
            }
        }

        public override void CalculateDialogue(IRoom room)
        {
            switch (CurrentState)
            {
                case (int)HardwareState.Unequipped:
                    DialogueContent = DialogueBuilder.Init()
                        .AddMainText($"<>{Env.ll}{Description}{Env.ll}This hardware is drifting through space.".Encode(this, LinkColors.Gatherable))
                        .AddOption("Pickup", new PickupHardwareAction(room.PlayerShip, this))
                        .Build();
                    break;
                case (int)HardwareState.Equipped:
                    DialogueContent = DialogueBuilder.Init()
                        .AddMainText($"<>{Env.ll}{Description}{Env.ll}Currently equipped to your ship.".Encode(this, LinkColors.Gatherable))
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

        public Hardware(FlexEntityDto dto, IRoom room) : base(dto, room)
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
            Description = "Quantum mapping device that identifies nearby space stations.";
        }
    }

    public class HazardDetector : Hardware
    {
        //public const string Key = "hazard_detector";

        public HazardDetector() : base()
        {
            Name = "Deep Field Scanner";
            Description = "A sophisticated navigation device capable of detecting nearby hazards.";
        }
    }

    public class MobDetector : Hardware
    {
        //public const string Key = "mob_detector";

        public MobDetector() : base()
        {
            Name = "Signature Detector";
            Description = "Standard-issue military hardware for detecting nearby weapon signatures.";
        }
    }

    public class GatheringBoost : Hardware
    {
        //public const string Key = "gathering_boost";

        public GatheringBoost() : base()
        {
            Name = "Robotic Manipulator";
            Description = "An advanced gathering tool capable of crushing rock.";
        }

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
            Description = "Advanced hull reinforcements that provide significant resistance to the hazards of space.";
        }
    }

    public class DropHardwareAction : SimpleAction
    {
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