using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using System;
using TextEncoding;

namespace Items
{
    public static class HardwareFactory
    {
        public static IHardware GetHardware(string key)
        {
            if (string.IsNullOrEmpty(key))
                return new EmptyHardware();

            switch (key)
            {
                case TownDetector.Key:
                    return new TownDetector();
                case HazardDetector.Key:
                    return new HazardDetector();
                case MobDetector.Key:
                    return new MobDetector();
                case GatheringBoost.Key:
                    return new GatheringBoost();
                case HazardMitigation.Key:
                    return new HazardMitigation();
            }

            throw new NotImplementedException();
        }
    }

    public interface IHardware
    {
        string Name { get; }
    }

    public class Hardware : FlexEntity, IHardware
    {
        private enum HardwareState
        {
            Unequipped = 0,
            Equipped = 1
        }

        public override void CalculateDialogue(IRoom room)
        {
            DialogueContent = DialogueBuilder.Init()
                .AddMainText($"Its a {GetLinkText()}")    
                .Build();
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

    public class EmptyHardware : IHardware
    {
        public const string Key = "empty_hardware";
        public string Name => "Empty Hardware";
    }

    public class TownDetector : IHardware
    {
        public const string Key = "town_detector";
        public string Name => "Cartographic Processor";
    }

    public class HazardDetector : Hardware
    {
        public const string Key = "hazard_detector";

        public HazardDetector() : base()
        {
            Name = "Deep Field Scanner";
        }
    }

    public class MobDetector : Hardware
    {
        public const string Key = "mob_detector";

        public MobDetector() : base()
        {
            Name = "Signature Detector";
        }
    }

    public class GatheringBoost : Hardware
    {
        public const string Key = "gathering_boost";

        public GatheringBoost() : base()
        {
            Name = "Robotic Manipulator";
        }

        private const float boost = 0.5f;

        public void ApplyBoost(ref int amount)
        {
            amount += (int)(amount * boost);
        }
    }

    public class HazardMitigation : IHardware
    {
        public const string Key = "hazard_mitigation";
        public string Name => "Hazard Plating";
    }
}