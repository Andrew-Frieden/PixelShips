using System.Linq;
using TextSpace.Items;
using TextSpace.Models.Actions;
using TextEncoding;

namespace TextSpace.Models.Dialogue
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

            public IDialogueBuilder AddOption(string text, IRoomAction action)
            {
                if (content.OptionAAction == null)
                    AddOptionA(text, action);
                else
                    AddOptionB(text, action);

                return this;
            }

            public IDialogueBuilder AddOptionA(string text, IRoomAction action)
            {
                AddTextA(text);
                AddActionA(action);
                return this;
            }

            public IDialogueBuilder AddOptionB(string text, IRoomAction action)
            {
                AddTextB(text);
                AddActionB(action);
                return this;
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

            public ABDialogueContent Build(IRoom room = null)
            {
                content.CalculateMode();

                if (content.OptionAAction != null)
                    content.OptionAAction.CalculateValid(room);

                if (content.OptionBAction != null)
                    content.OptionBAction.CalculateValid(room);

                return content;
            }
        }
        
        public static IDialogueBuilder Init()
        {
            return new DialogueBuild();
        }

        public static ABDialogueContent PlayerAttackDialogue(string mainText, IRoomActor target, IRoom room)
        {
            var playerShip = room.PlayerShip;
            var lightWeapon = playerShip.LightWeapon;
            var heavyWeapon = playerShip.HeavyWeapon;

            return Init()
                .AddMainText(mainText)
                .AddOption(lightWeapon.Name, lightWeapon.GetAttackAction(room, playerShip, target))
                .AddOption(heavyWeapon.Name, heavyWeapon.GetAttackAction(room, playerShip, target))
                    .Build(room);
        }

        public static ABDialogueContent EmptyDialogue(IRoom room)
        {
            return Init()
                .AddMainText("The scanner signature was lost.")
                .Build(room);
        }
    }
    
    public interface IDialogueBuilder
    {
        IDialogueBuilder AddMainText(string text);
        IDialogueBuilder AddTextA(string text);
        IDialogueBuilder AddTextB(string text);
        IDialogueBuilder AddActionA(IRoomAction action);
        IDialogueBuilder AddActionB(IRoomAction action);
        IDialogueBuilder AddOption(string text, IRoomAction action);
        IDialogueBuilder AddOptionA(string text, IRoomAction action);
        IDialogueBuilder AddOptionB(string text, IRoomAction action);

        ABDialogueContent Build(IRoom room = null);
    }
}