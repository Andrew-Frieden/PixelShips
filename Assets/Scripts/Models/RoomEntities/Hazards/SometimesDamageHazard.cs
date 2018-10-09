using System;
using System.Collections.Generic;
using Helpers;
using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.RoomEntities.Hazards;
using TextEncoding;
using Models.Stats;

public class SometimesDamageHazard : Hazard
{
    public override IRoomAction MainAction(IRoom room)
    {
        var damageOccurred = Stats[StatKeys.HazardDamageChance] > UnityEngine.Random.Range(1, 100);

        if (damageOccurred)
        {
            return new HazardDamageAction(this, room.PlayerShip, Stats[StatKeys.HazardDamageAmount], Values[ValueKeys.HazardDamageText]);
        }

        return new DoNothingAction(this);
    }

    public SometimesDamageHazard(FlexEntityDto dto, IRoom room) : base(dto, room)
    {
    }

    public SometimesDamageHazard(Dictionary<string, int> stats, Dictionary<string, string> values) : base(stats, values)
    {
        IsHostile = true;
    }

    private class HazardDamageAction : SimpleAction
    {
        private int BaseDamage
        {
            get
            {
                return Stats[StatKeys.BaseDamageKey];
            }
            set
            {
                Stats[StatKeys.BaseDamageKey] = value;
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
                [StatKeys.BaseDamageKey] = amount
            };

            Values = new Dictionary<string, string>
            {
                [ValueKeys.HazardDamageText] = flavorText
            };
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            var actualDamage = BaseDamage;
            
            Target.TakeDamage(actualDamage);
            
            var resultText = string.Format(Values[ValueKeys.HazardDamageText], actualDamage);
            
            return new List<TagString>()
            {
                new TagString()
                {
                    Text = resultText.Encode(Source.GetLinkText(), Source.Id, LinkColors.Hazard),
                    Tags = new List<EventTag> { EventTag.Damage }
                }
            };
        }
    }
}