using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
using System.Collections.Generic;
using TextEncoding;
using UnityEngine;

public class VerdantInformantMob : FlexEntity
{
    public VerdantInformantMob() : base()
    {
        Name = "Verdant Informant";
        IsHostile = false;
        IsAttackable = true;
        Values[ValueKeys.LookText] = "A <> is in the sector, guns at the ready.";
        Values[ValueKeys.LookTextAggro] = "A <> is maneuvering to attack position.";
        Stats[StatKeys.Hull] = 8;
    }

    public override ABDialogueContent CalculateDialogue(IRoom room)
    {
        return DialogueBuilder.PlayerAttackDialogue("Verdant Informant\n" +
            "Hull: " +Stats[StatKeys.Hull]+ " / 8\n" +
            "This ship is definitely up to something, but it's very difficult to tell what.\n" +
            "You can probably take them.", this, room);
    }

    public override TagString GetLookText()
    {
        if (IsHostile)
        {
            return new TagString()
            {
                Text = Values[ValueKeys.LookTextAggro].Encode(Name, Id, LinkColors.HostileEntity),
            };
        }
        
        return new TagString()
        {
            Text = Values[ValueKeys.LookText].Encode(Name, Id, LinkColors.CanCombatEntity),
        };
    }

    public override IRoomAction MainAction(IRoom room)
    {
        if (!IsHostile)
        {
            return new BecomeHostileIfNeutralAction(this);
        }
        else if (IsHostile)
        {
            return new CreateDelayedAttackActorAction(this, room.PlayerShip, 2, 8, "Leech Torpedo");
        }
        return new DoNothingAction(this);
    }

    public VerdantInformantMob(FlexEntityDto dto, IRoom room) : base(dto, room)
    {
    }

    public VerdantInformantMob(Dictionary<string, int> stats, Dictionary<string, string> values) : base(stats, values)
    {
    }

    public override IRoomAction CleanupStep(IRoom room)
    {
        if (IsDestroyed)
        {
            return new DropKelpFiberAction(this);
        }
        return new DoNothingAction(this);
    }

    private class BecomeHostileIfNeutralAction : SimpleAction
    {
        public BecomeHostileIfNeutralAction(SimpleActionDto dto, IRoom room) : base(dto, room)
        {
        }

        public BecomeHostileIfNeutralAction(IRoomActor src)
        {
            Source = src;
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            if (!Source.IsHostile && (Random.Range(0,10) >= 5))
            {
                Source.IsHostile = true;
                
                return new List<TagString>()
                {
                    new TagString()
                    {
                        Text = "<> warms up their weapon systems".Encode(Source.GetLinkText(), Source.Id, LinkColors.HostileEntity),
                    }
                };
            }
            return new List<TagString>();
        }
    }

    private class DropKelpFiberAction : SimpleAction
    {
        public DropKelpFiberAction(IRoomActor src) : base()
        {
            Source = src;
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {

            var explosion = $"The <>'s hull splinters apart!".Encode(Source, LinkColors.HostileEntity).Tag();


            if (5 < Random.Range(0, 10))
            {
                var drop = new ScrapGatherable("Kelp Fiber");
                room.Entities.Add(drop);
                var dropped = $"Earthy <> spews from the wreckage.".Encode(drop, LinkColors.Gatherable).Tag();
                return new TagString[] { explosion, dropped };
            }



            return new TagString[] { explosion };
        }
    }
}