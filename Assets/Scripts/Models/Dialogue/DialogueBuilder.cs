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