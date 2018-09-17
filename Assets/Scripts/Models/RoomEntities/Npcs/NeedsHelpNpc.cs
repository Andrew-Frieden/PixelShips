using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using TextEncoding;

namespace Models
{
    public partial class NeedsHelpNpc : FlexEntity
    {
        private enum NpcState
        {
            NeedsHelp = 0,
            HelpSuccess = 1,
            HelpFail = 2
        }
        
        private Dictionary<NpcState, string> LookText = new Dictionary<NpcState, string>
        {
            { NpcState.NeedsHelp, "You recieve a hail from a <> that appears to be tangled in the kelp." },
            { NpcState.HelpSuccess, "There is a slowly cruising <> weaving between the kelp." },
            { NpcState.HelpFail, "A <> is irreparably trapped in the dense kelp." }
        };
        
        public NeedsHelpNpc(FlexEntityDto dto, IRoom room) : base(dto, room)
        {
        }

        public NeedsHelpNpc(string name = "NCA Patrol Officer") : base()
        {
            Name = name;
        }

        public override IRoomAction GetNextAction(IRoom room)
        {
            return new DoNothingAction(this);
        }

        public override ABDialogueContent CalculateDialogue(IRoom room)
        {
            switch (CurrentState)
            {
                case (int)NpcState.NeedsHelp:
                    return DialogueBuilder.Init()
                        .AddMainText("Meow! I need some help.")
                        .AddTextA("Try to jumpstart")
                            .AddActionA(new HelpRiskyAction(room.PlayerShip, this))
                        .AddTextB("Give their vessel a nudge")
                            .AddActionB(new HelpSafeAction(room.PlayerShip, this))
                        .Build();
                case (int)NpcState.HelpSuccess:
                    return DialogueBuilder.Init()
                        .AddMainText("Yay thanks for the help!")
                        .Build();
                case (int)NpcState.HelpFail:
                    return DialogueBuilder.Init()
                        .AddMainText("Hey you tried your best. I'll call for backup.")
                        .Build();
            }

            throw new NotSupportedException();
        }

        public override string GetLookText()
        {
            return LookText[(NpcState)CurrentState].Encode(Name, Id, LinkColors.NPC);
        }
    }

    public partial class NeedsHelpNpc
    {
        private class HelpRiskyAction : SimpleAction
        {
            public HelpRiskyAction(SimpleActionDto dto, IRoom room) : base(dto, room)
            {
            }
            
            public HelpRiskyAction(IRoomActor src, IRoomActor target)
            {
                Source = src;
                Target = target;
            }

            public override IEnumerable<string> Execute(IRoom room)
            {
                var succeeded = false;
                
                if (Source.Stats.ContainsKey("captainship"))
                {
                    var captainship = Source.Stats["captainship"];
                    if (captainship > 5)
                    {
                        succeeded = true;                        
                    }
                }
                
                if (succeeded)
                {
                    Target.ChangeState((int)NpcState.HelpSuccess);
                    Source.Stats["resourcium"] += 15;
                    return new string[] { "Hey it worked! Tabby Officer gives you 15 resourcium." };
                }
                else
                {
                    Target.ChangeState((int)NpcState.HelpFail);
                    return new string[] { "Uh oh. That didn't work well." };
                }
            }
        }
        
        private class HelpSafeAction : SimpleAction
        {
            public HelpSafeAction(SimpleActionDto dto, IRoom room) : base(dto, room)
            {
            }
            
            public HelpSafeAction(IRoomActor src, IRoomActor target)
            {
                Source = src;
                Target = target;
            }

            public override IEnumerable<string> Execute(IRoom room)
            {
                var succeeded = false;
                
                if (Source.Stats.ContainsKey("captainship"))
                {
                    var captainship = Source.Stats["captainship"];
                    if (captainship > 1)
                    {
                        succeeded = true;                        
                    }
                }
                
                if (succeeded)
                {
                    Target.ChangeState((int)NpcState.HelpSuccess);
                    Source.Stats["resourcium"] += 5;
                    return new string[] { "Hey it worked! Tabby Officer gives you 5 resourcium." };
                }
                else
                {
                    Target.ChangeState((int)NpcState.HelpFail);
                    return new string[] { "Uh oh. That didn't work well." };
                }
            }
        }
    }
}