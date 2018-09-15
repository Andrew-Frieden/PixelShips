using System;
using Models.Actions;

namespace Models.Dialogue
{
    public static class DialogueBuilder
    {
        private class DialogueBuild : IDialogueBuilder
        {
            private readonly ABDialogueContent content;

            public DialogueBuild()
            {
                content = new ABDialogueContent();
            }

            public IDialogueBuilder AddActionA(IRoomAction action)
            {
                content.OptionAAction = action;
                return this;
            }

            public IDialogueBuilder AddActionB(IRoomAction action)
            {
                content.OptionBAction = action;
                return this;
            }

            public IDialogueBuilder AddMainText(string text)
            {
                content.MainText = text;
                return this;
            }

            public IDialogueBuilder AddTextA(string text)
            {
                content.OptionAText = text;
                return this;
            }

            public IDialogueBuilder AddTextB(string text)
            {
                content.OptionBText = text;
                return this;
            }

            public ABDialogueContent Build()
            {
                return content;
            }

            public IDialogueBuilder SetMode(ABDialogueMode mode)
            {
                content.Mode = mode;
                return this;
            }
        }
        
        public static IDialogueBuilder Init()
        {
            return new DialogueBuild();
        }

        public static ABDialogueContent PlayerAttackDialogue(string mainText, IRoomActor target, IRoom room)
        {
            return Init()
                .AddMainText(mainText)
                .AddTextA("Pulse Lasers")
                    .AddActionA(new AttackAction(room.PlayerShip, target, 2))
                .AddTextB("Plasma Torpedo")
                    .AddActionB(new AttackAction(room.PlayerShip, target, 8))
                    .Build();
        }

        public static ABDialogueContent PlayerNavigateDialogue(IRoom room)
        {
            if (room.PlayerShip.WarpDriveReady)
            {
                return Init()
                    .AddMainText(room.GetLookText() + Environment.NewLine + Environment.NewLine + "Your warp drive is fully charged.")
                    .AddTextA("Warp to room A.")
                    .AddActionA(new WarpAction(room.Exits[0]))
                    .AddTextB("Warp to room B.")
                    .AddActionB(new WarpAction(room.Exits[1]))
                    .Build();
            }

            return Init()
                .AddMainText(room.GetLookText() + Environment.NewLine + Environment.NewLine + "Your warp drive is cold.")
                .AddTextA("Spin up your warp drive.")
                .AddActionA(new CreateWarpDriveActorAction(1))
                .SetMode(ABDialogueMode.ACancel)
                .Build();
        }
    }
    
    public interface IDialogueBuilder
    {
        IDialogueBuilder AddMainText(string text);
        IDialogueBuilder AddTextA(string text);
        IDialogueBuilder AddTextB(string text);
        IDialogueBuilder AddActionA(IRoomAction action);
        IDialogueBuilder AddActionB(IRoomAction action);
        IDialogueBuilder SetMode(ABDialogueMode mode);
        ABDialogueContent Build();
    }
}