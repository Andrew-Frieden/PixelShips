using System;
using System.Linq;
using Models.Actions;
using TextEncoding;

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

            public ABDialogueContent Build()
            {
                if (content.OptionAAction != null && content.OptionBAction != null)
                    content.Mode = ABDialogueMode.ABCancel;
                else if (content.OptionAAction != null && content.OptionBAction == null)
                    content.Mode = ABDialogueMode.ACancel;
                else if (content.OptionAAction == null && content.OptionBAction == null)
                    content.Mode = ABDialogueMode.Cancel;

                return content;
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
                    .AddActionA(new AttackAction(room.PlayerShip, target, 2, "Pulse Lasers"))
                .AddTextB("Plasma Torpedo")
                    .AddActionB(new CreateDelayedAttackActorAction(room.PlayerShip, target,1, 8, "Plasma Torpedo"))
                    .Build();
        }

        public static ABDialogueContent PlayerNavigateDialogue(IRoom room)
        {
            if (room.PlayerShip.WarpDriveReady)
            {
                var tempA = room.Exits.First();
                var tempB = room.Exits.Last();
                var aText = GetRoomExitText(tempA);
                var bText = GetRoomExitText(tempB);

                return Init()
                    .AddMainText($"{room.Description}{Env.ll}Your warp drive is fully charged.")
                    .AddOption(aText, new WarpAction(tempA))
                    .AddOption(bText, new WarpAction(tempB))
                    .Build();
            }

            return Init()
                .AddMainText($"{room.Description}{Env.ll}Your warp drive is cold.")
                .AddTextA("Spin up warp drive.")
                .AddActionA(new WarpDriveReadyAction(room.PlayerShip))
                .Build();
        }

        private static string GetRoomExitText(RoomTemplate t)
        {
            var text = $"Warp to {Room.GetNameForFlavor(t.Flavor)}{Env.ll}";

            if (t.ActorFlavors.Contains(RoomActorFlavor.Hazard))
            {
                text += "Hazard Detected" + Env.l;
            }

            if (t.ActorFlavors.Contains(RoomActorFlavor.Mob))
            {
                text += "Hostile Detected" + Env.l;
            }

            if (t.ActorFlavors.Contains(RoomActorFlavor.Town))
            {
                text += "Nearby Starport Detected" + Env.l;
            }

            return text;
        }

        public static ABDialogueContent EmptyDialogue()
        {
            return Init()
                .AddMainText("The scanner signature was lost.")
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
        IDialogueBuilder AddOption(string text, IRoomAction action);
        IDialogueBuilder AddOptionA(string text, IRoomAction action);
        IDialogueBuilder AddOptionB(string text, IRoomAction action);

        ABDialogueContent Build();
    }
}