using UnityEngine;
using System.Collections;
using TMPro;
using PixelShips.Verse;
using System.Collections.Generic;
using System;
using System.Linq;
using PixelSpace.Models.SharedModels;
using PixelSpace.Models.SharedModels.Helpers;
using TMPro;

namespace PixelShips.Widgets
{
    public class MapButton : BaseWidget
    {
        private IEnumerable<SpaceAction> ExitActions;
        private IGameState _state;

        public TMP_Text Text;

        protected override void OnVerseUpdate(IGameState state)
        {
            ExitActions = state.UserActions.Where(a => a.Name == "start_instant_jump");
            _state = state;
        }

        public void PrintExits()
        {
            if (ExitActions == null)
                return;

            foreach (var spaceAction in ExitActions)
            {
                var jumpAction = spaceAction as StartInstantJumpAction;
                //Debug.Log(string.Format("Nav [{0}]", jumpAction.TargetRoomId));

                PrintJump(jumpAction);
            }
        }

        private void PrintJump(StartInstantJumpAction jump)
        {
            //var targetRoom = _state.Rooms.Single(r => r.Id == jump.TargetRoomId);
            var jumpLink = string.Format("[Sector {0}]", jump.TargetRoomId.Substring(0, 4));
            jumpLink = jumpLink.AddLink(jump.TargetRoomId).AddColor("orange");
            var jumpText = string.Format("Nav point at {0}", jumpLink);
            Text.text += jumpText + Environment.NewLine;
        }
    }

    public static class TextHelpers
    {
        public static string AddLink(this string text, string id)
        {
            return string.Format("<link={0}>{1}</link>", id, text);
        }

        public static string AddColor(this string text, string color)
        {
            return string.Format("<color={0}>{1}</color>", color, text);
        }
    }
}
