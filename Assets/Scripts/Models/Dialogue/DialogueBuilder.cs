﻿using System;
using System.Linq;
using Items;
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
                content.CalculateMode();
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
                .AddOption(lightWeapon.Name, lightWeapon.GetAction(playerShip, target))
                .AddOption(heavyWeapon.Name, heavyWeapon.GetAction(playerShip, target))
                    .Build();
        }

        public static ABDialogueContent PlayerNavigateDialogue(IRoom room)
        {
            if (room.PlayerShip.WarpDriveReady)
            {
                var templateA = room.Exits.First();
                var templateB = room.Exits.Last();
                var aText = GetRoomExitText(room.PlayerShip, templateA);
                var bText = GetRoomExitText(room.PlayerShip, templateB);

                return Init()
                    .AddMainText($"{room.Description}{Env.ll}Your warp drive is fully charged.")
                    .AddOption(aText, new WarpAction(templateA))
                    .AddOption(bText, new WarpAction(templateB))
                    .Build();
            }

            return Init()
                .AddMainText($"{room.Description}{Env.ll}Your warp drive is cold.")
                .AddTextA("Spin up warp drive.")
                .AddActionA(new WarpDriveReadyAction(room.PlayerShip))
                .Build();
        }

        private static string GetRoomExitText(CommandShip ship, RoomTemplate t)
        {
            var text = $"Warp to {Room.GetNameForFlavor(t.Flavor)}{Env.ll}";

            if (t.ActorFlavors.Contains(RoomActorFlavor.Hazard) 
                && ship.CheckHardware<HazardDetector>())
            {
                text += "Hazard Detected" + Env.l;
            }

            if (t.ActorFlavors.Contains(RoomActorFlavor.Mob)
                && ship.CheckHardware<MobDetector>())
            {
                text += "Hostile Detected" + Env.l;
            }

            if (t.ActorFlavors.Contains(RoomActorFlavor.Town)
                && ship.CheckHardware<TownDetector>())
            {
                text += "Starport Detected" + Env.l;
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