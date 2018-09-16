using Links.Colors;
using Models;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using Models.Stats;
using System.Collections.Generic;
using TextEncoding;

public class PirateMob : FlexEntity
{
    public PirateMob() : base()
    {
        Name = "Rogue Privateer";
        IsHostile = false;
        IsAttackable = false;
        Values[ValueKeys.LookText] = "You recieve a hail from a menacing <>";
        Values[ValueKeys.LookTextAggro] = "A <> is maneuvering to attack position";
        Stats[StatKeys.Hull] = 5;
    }

    public override ABDialogueContent CalculateDialogue(IRoom room)
    {
        if (IsHostile)
        {
            return DialogueBuilder.PlayerAttackDialogue("The privateer's weapons are already spun up and locked on to you!", this, room);
        }
        else if (IsAttackable)
        {
            return DialogueBuilder.PlayerAttackDialogue("A rusty privateer cruises through the sector.", this, room);
        }
        else
        {
            return DialogueBuilder.Init()
                        .AddMainText(@"A crackle comes through the comms:

Empty your cargo or we'll dust ya!")
                        .AddTextA("Transfer 100 resourcium")
                            .AddActionA(new GetRobbedAction(room.PlayerShip, this, 100)) //  player loses 100 resourcium and pirate ship to CanCombat = true
                        .AddTextB("Talk your way out of it")
                            .AddActionB(new SmoothTalkAction(room.PlayerShip, this, 10))  //  if successful, this action will set the pirate ship to CanCombat = true, failure sets pirate to IsHostile = true
                        .Build();
        }
    }

    public override string GetLookText()
    {
        if (IsHostile)
        {
            return Values[ValueKeys.LookTextAggro].Encode(Name, Id, LinkColors.HostileEntity);
        }
        else if (IsAttackable)
        {
            return Values[ValueKeys.LookText].Encode(Name, Id, LinkColors.CanCombatEntity);
        }
        else
        {
            return Values[ValueKeys.LookText].Encode(Name, Id, LinkColors.NPC);
        }
    }

    public override IRoomAction GetNextAction(IRoom room)
    {
        if (!IsHostile && !IsAttackable)
        {
            return new BecomeHostileIfNeutralAction(this);
        }
        else if (IsHostile)
        {
            return new AttackAction(this, room.PlayerShip, 2, "Pirate Cannon");
        }
        return new DoNothingAction(this);
    }

    public PirateMob(FlexEntityDto dto, IRoom room) : base(dto, room)
    {
    }

    public PirateMob(Dictionary<string, int> stats, Dictionary<string, string> values) : base(stats, values)
    {
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

        public override IEnumerable<string> Execute(IRoom room)
        {
            if (!Source.IsHostile && !Source.IsAttackable)
            {
                Source.IsHostile = true;
                return new string[] { "<> warms up their weapon systems".Encode(Source.GetLinkText(), Source.Id, LinkColors.HostileEntity) };
            }
            return new string[] { };
        }
    }

    private class SmoothTalkAction : SimpleAction
    {
        private const string SmoothTalkDcKey = "smooth_talk_dc";

        private int Dc
        {
            get
            {
                return Stats[SmoothTalkDcKey];
            }
            set
            {
                Stats[SmoothTalkDcKey] = value;
            }
        }

        public SmoothTalkAction(SimpleActionDto dto, IRoom room) : base(dto, room)
        {
        }

        public SmoothTalkAction(IRoomActor src, IRoomActor target, int dc)
        {
            Source = src;
            Target = target;
            Stats = new Dictionary<string, int>();
            Dc = dc;
        }

        public override IEnumerable<string> Execute(IRoom room)
        {
            var stat = Source.Stats[StatKeys.Captainship];
            var roll = stat + UnityEngine.Random.Range(1, 21);

            if (roll >= Dc)
            {
                Target.IsHostile = false;
                Target.IsAttackable = true;
                return new string[] { "The <> decides its not worth the trouble and changes course.".Encode(Target.GetLinkText(), Target.Id, LinkColors.CanCombatEntity) };
            }
            else
            {
                Target.IsHostile = true;
                return new string[] { "The <> captain mocks your attempt and moves to an attack vector.".Encode(Target.GetLinkText(), Target.Id, LinkColors.HostileEntity) };
            }
        }
    }

    private class GetRobbedAction : SimpleAction
    {
        private const string MobbedAmountKey = "mobbed_amount";

        private int Amount
        {
            get
            {
                return Stats[MobbedAmountKey];
            }
            set
            {
                Stats[MobbedAmountKey] = value;
            }
        }

        public GetRobbedAction(SimpleActionDto dto, IRoom room) : base(dto, room)
        {
        }

        public GetRobbedAction(IRoomActor src, IRoomActor target, int amount)
        {
            Source = src;
            Target = target;
            Stats = new Dictionary<string, int>();
            Amount = amount;
        }

        public override IEnumerable<string> Execute(IRoom room)
        {
            Target.IsHostile = false;
            Target.IsAttackable = true;

            if (Source.Stats[StatKeys.Resourcium] < Amount)
            {
                Amount = Source.Stats[StatKeys.Resourcium];
            }

                Source.Stats[StatKeys.Resourcium] -= Amount;


            return new string[] { string.Format("You transfer {0} resourcium to the <>", Amount).Encode(Target.GetLinkText(), Target.Id, LinkColors.CanCombatEntity) };
        }
    }
}