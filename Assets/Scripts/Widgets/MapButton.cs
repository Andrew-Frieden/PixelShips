using UnityEngine;
using System.Collections;
using TMPro;
using PixelShips.Verse;
using System.Collections.Generic;
using System;
using System.Linq;
using PixelSpace.Models.SharedModels;
using PixelShips.Helpers;

namespace PixelShips.Widgets
{
    public class MapButton : BaseWidget
    {
        private IEnumerable<SpaceAction> ExitActions;
        private IGameState _state;

        public TextMeshProUGUI Text;

        protected override void OnVerseUpdate(IGameState state)
        {
            _state = state;

            if (state.UserActions == null)
                return;

            ExitActions = state.UserActions.Where(a => a.Name == "start_instant_jump");
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
            jumpLink = jumpLink.GetActiveRoomText(jump.TargetRoomId);
            var jumpText = string.Format("Nav point at {0}", jumpLink);
            Text.text += jumpText + Environment.NewLine;
        }
    }
}
