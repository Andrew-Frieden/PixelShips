using System.Collections.Generic;
using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using TextEncoding;

public class HomeworldNpc : FlexEntity
{
    public override void CalculateDialogue(IRoom room)
    {
        DialogueContent = DialogueBuilder.Init()
            .AddMainText(DialogueText.Encode(Name, Id, LinkColors.Player))
            .Build();
    }

    public override TagString GetLookText()
    {
        return new TagString()
        {
            Text = "Your beautiful homeworld <> orbits a familiar star.".Encode(Name, Id, LinkColors.Player),
        };
    }

    public HomeworldNpc(FlexEntityDto dto) : base(dto) { }

    public HomeworldNpc(Homeworld world)
    {
        Name = world.PlanetName;
        DialogueText = $"The {world.Description.ToLower()} world of {world.PlanetName}, crucible of your clan.{Env.ll}It will be missed.";
    }
}
