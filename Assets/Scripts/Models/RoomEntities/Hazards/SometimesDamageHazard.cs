using TextSpace.Models;
using TextSpace.Models.Actions;
using TextSpace.Models.Dtos;
using TextSpace.Models.RoomEntities.Hazards;
using TextSpace.Models.Stats;

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

    public SometimesDamageHazard(FlexData data) : base(data)
    {
    }
}