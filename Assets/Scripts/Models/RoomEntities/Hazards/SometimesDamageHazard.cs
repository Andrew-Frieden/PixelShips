using System;
using System.Collections.Generic;
using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using TextEncoding;
using Models.Stats;
using Links.Colors;

public class SometimesDamageHazard : FlexEntity
{
    public override ABDialogueContent CalculateDialogue(IRoom room)
    {
        return DialogueBuilder.Init()
            .AddMainText(Values[ValueKeys.DialogueText].Encode(Name, Id, LinkColors.Hazard))
            .SetMode(ABDialogueMode.Cancel)
            .Build();
    }

    public override string GetLookText()
    {
        return Values[ValueKeys.LookText].Encode(Name, Id, LinkColors.Hazard);
    }

    public override IRoomAction GetNextAction(IRoom room)
    {
        var damage_occurred = Stats[StatKeys.HazardDamageChance] > UnityEngine.Random.Range(1, 100);

        if (damage_occurred)
        {
            return new HazardDamageAction(this, room.PlayerShip, Stats[StatKeys.HazardDamageAmount], Values[ValueKeys.HazardDamageText]);
        }
        else
        {
            return new DoNothingAction(this);
        }
    }

    public SometimesDamageHazard(FlexEntityDto dto, IRoom room) : base(dto, room)
    {
    }

    public SometimesDamageHazard(Dictionary<string, int> stats, Dictionary<string, string> values) : base(stats, values)
    {
    }

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

        public HazardDamageAction(IRoomActor source, IRoomActor target, int amount, string flavorText)
        {
            Source = source;
            Target = target;

            Stats = new Dictionary<string, int>
            {
                [BaseDamageKey] = amount
            };

            Values = new Dictionary<string, string>
            {
                [ValueKeys.HazardDamageText] = flavorText
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
            
            PlayerTookDamage(new PlayerTookDamageEventArgs(actualDamage));

            //var exampleText = "An energy surge from a <> scorches your hull for {0} damage!";
            var resultText = string.Format(Values[ValueKeys.HazardDamageText], actualDamage);
            return new string[] { resultText.Encode(Source.GetLinkText(), Source.Id, LinkColors.Hazard) };
        }
    }
}