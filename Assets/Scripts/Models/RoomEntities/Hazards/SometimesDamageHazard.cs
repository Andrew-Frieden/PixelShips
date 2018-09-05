using System;
using System.Collections.Generic;
using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using TextEncoding;
using static Models.Stats.StatKeys;

public class ExampleHazardFlexEntity : FlexEntity
{
    public override ABDialogueContent CalculateDialogue(IRoom room)
    {
        return DialogueBuilder.Init()
            .AddMainText("The plasma storm looks pretty dangerous. There isn't much you can do about it.")
            .SetMode(ABDialogueMode.Acknowledge)
            .Build();
    }

    public override string GetLookText()
    {
        if (Stats[HazardDamageAmount] > 5)
            return "A <> churns with terrifying power.".Encode(Name, Id, "purple");

        return "A <> crackles through this sector.".Encode(Name, Id, "purple");
    }

    public override IRoomAction GetNextAction(IRoom room)
    {
        var damage_occurred = Stats[HazardDamageChance] > UnityEngine.Random.Range(1, 100);

        if (damage_occurred)
        {
            return new HazardDamageAction(this, room.PlayerShip, Stats[HazardDamageAmount]);
        }
        else
        {
            return new DoNothingAction(this);
        }
    }

    public ExampleHazardFlexEntity(FlexEntityDto dto, IRoom room) : base(dto, room)
    {
    }

    public ExampleHazardFlexEntity(int damage, int chance = 50)
    {
        Name = "Plasma Storm";
        Stats = new Dictionary<string, int>
        {
            [HazardDamageAmount] = damage,
            [HazardDamageChance] = chance
        };
    }

    private const string HazardDamageAmount = "hazard_damage_amount";
    private const string HazardDamageChance = "hazard_damage_chance";

    private class HazardDamageAction : SimpleAction
    {
        private const string BaseDamageKey = "base_damage";
        private int BaseDamage
        {
            get
            {
                return Stats[BaseDamageKey];
            }
            set
            {
                Stats[BaseDamageKey] = value;
            }
        }

        public HazardDamageAction(SimpleActionDto dto, IRoom room) : base(dto, room)
        {
        }

        public HazardDamageAction(IRoomActor source, IRoomActor target, int amount)
        {
            Source = source;
            Target = target;

            Stats = new Dictionary<string, int>
            {
                [BaseDamageKey] = amount
            };
        }

        public override IEnumerable<string> Execute(IRoom room)
        {
            var actualDamage = BaseDamage;

            if (Target.Stats.ContainsKey(StatKeys.ExampleDamageMitigationStat))
            {
                actualDamage -= Target.Stats[StatKeys.ExampleDamageMitigationStat];
                actualDamage = Math.Max(actualDamage, 1);
            }

            Target.Stats[StatKeys.Hull] -= actualDamage;
            return new string[] { $"An energy surge from a <> scorches your hull for {actualDamage} damage!".Encode(Source.GetLinkText(), Source.Id, "purple") };
        }
    }
}