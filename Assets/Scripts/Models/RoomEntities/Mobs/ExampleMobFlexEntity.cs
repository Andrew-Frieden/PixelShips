using Models;
using Models.Actions;
using Models.Dtos;

public class ExampleMobFlexEntity : FlexEntity
{
    public override ABDialogueContent CalculateDialogue(IRoom room)
    {
        throw new System.NotImplementedException();
    }

    public override string GetLookText()
    {
        throw new System.NotImplementedException();
    }

    public override IRoomAction GetNextAction(IRoom room)
    {
        throw new System.NotImplementedException();
    }

    public ExampleMobFlexEntity(FlexEntityDto dto, IRoom room) : base(dto, room)
    {
    }
}