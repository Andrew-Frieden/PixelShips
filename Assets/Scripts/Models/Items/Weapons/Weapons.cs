using Models;
using Models.Actions;
using System;

namespace Items
{
    public static class WeaponFactory
    {
        public static IWeapon GetWeapon(string weapon)
        {
            switch (weapon)
            {
                case PulseLaser.Key:
                    return new PulseLaser();
                case PlasmaTorpedo.Key:
                    return new PlasmaTorpedo();
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }

    public interface IWeapon
    {
        string Name { get; }
        IRoomAction GetAction(IRoomActor src, IRoomActor target);
    }

    //  TODO make an injectable version of these weapons
    public class PulseLaser : IWeapon
    {
        public const string Key = "pulse_laser";
        public string Name => "Pulse Laser";

        public IRoomAction GetAction(IRoomActor src, IRoomActor target)
        {
            return new AttackAction(src, target, 2, Name);
        }
    }

    public class PlasmaTorpedo : IWeapon
    {
        public const string Key = "plasma_torpedo";
        public string Name => "Plasma Torpedo";

        public IRoomAction GetAction(IRoomActor src, IRoomActor target)
        {
            return new CreateDelayedAttackActorAction(src, target, 1, 8, Name);
        }
    }
}

