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

    public SometimesDamageHazard(FlexEntityDto dto) : base(dto)
    {
    }

    public SometimesDamageHazard(Dictionary<string, int> stats, Dictionary<string, string> values) : base(stats, values)
    {
        IsHostile = true;
    }
}