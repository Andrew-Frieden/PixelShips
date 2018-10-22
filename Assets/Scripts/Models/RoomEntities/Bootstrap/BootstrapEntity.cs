using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using TextEncoding;

namespace Models
{
    public partial class BootstrapEntity : FlexEntity
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
        
        public BootstrapEntity(FlexEntityDto dto) : base(dto)
        {
        }

        public BootstrapEntity(string name = "NCA Patrol Officer") : base()
        {
            Name = name;
        }

        public override IRoomAction MainAction(IRoom room)
        {
            return new DoNothingAction(this);
        }

        public override void CalculateDialogue(IRoom room)
        {
            switch (CurrentState)
            {
                case (int)NpcState.NeedsHelp:
                    //DialogueContent = DialogueBuilder.Init()
                    //    .AddMainText("Meow! I need some help.")
                    //    .AddTextA("Try to jumpstart")
                    //        .AddActionA(new HelpRiskyAction(room.PlayerShip, this))
                    //    .AddTextB("Give their vessel a nudge")
                    //        .AddActionB(new HelpSafeAction(room.PlayerShip, this))
                    //    .Build(room);
                    break;
                case (int)NpcState.HelpSuccess:
                    DialogueContent = DialogueBuilder.Init()
                        .AddMainText("Yay thanks for the help!")
                        .Build(room);
                    break;
                case (int)NpcState.HelpFail:
                    DialogueContent = DialogueBuilder.Init()
                        .AddMainText("Hey you tried your best. I'll call for backup.")
                        .Build(room);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public override TagString GetLookText()
        {
            return new TagString()
            {
                Text = LookText[(NpcState)CurrentState].Encode(Name, Id, LinkColors.NPC)
            };
        }
    }
}