using UnityEngine;
using System.Collections;
using TMPro;
using PixelShips.Verse;
using System.Collections.Generic;
using System;
using System.Linq;
using PixelShips.Helpers;
using PixelSpace.Models.SharedModels;
using PixelSpace.Models.SharedModels.Helpers;

namespace PixelShips.Widgets
{
    public class FocusTextWidget : BaseWidget
    {
        public TextMeshProUGUI DisplayText;
        private string defaultFocusText;
        private IGameState _gameState;

        public JumpButton JumpBtn;
        public CancelButton CancelBtn;
        public MapButton MapBtn;
        public ScanButton ScanBtn;

        private void Start()
        {
            if (DisplayText == null)
                DisplayText = GetComponentInChildren<TextMeshProUGUI>();

            DisplayText.text = string.Empty;
        }

        public void SetText(string text)
        {
            DisplayText.text = text;
        }

        public void SetSelectedObjectId(string spaceObjectId)
        {
            var gameState = _gameState;

            foreach (var ship in gameState.Ships)
            {
                if (ship.Id == spaceObjectId)
                {
                    DisplayText.text = ship.GetActiveTextTag(true);
                    return;
                }
            }

            foreach (var exitId in gameState.UserRoom.ExitIds)
            {
                if (exitId == spaceObjectId)
                {
                    DisplayText.text = ActiveTextHelpers.GetActiveRoomText(exitId);

                    var jumpActions = gameState.UserActions.Where(a => a is StartInstantJumpAction).Select(j => j as StartInstantJumpAction);
                    //gameState.UserActions.ForEach(j => Debug.Log(j));
                    var jumpAction = jumpActions.Where(a => a.TargetRoomId == exitId).Single();

                    JumpBtn.JumpAction = jumpAction;

                    JumpBtn.gameObject.SetActive(true);
                    CancelBtn.gameObject.SetActive(true);
                    MapBtn.gameObject.SetActive(false);
                    ScanBtn.gameObject.SetActive(false);

                    return;
                }
            }

            DisplayText.text = "[empty]";
        }

        public void SetDefaultText()
        {
            DisplayText.text = defaultFocusText;

            JumpBtn.gameObject.SetActive(false);
            CancelBtn.gameObject.SetActive(false);
            MapBtn.gameObject.SetActive(true);
            ScanBtn.gameObject.SetActive(true);
        }

        protected override void OnVerseUpdate(IGameState state)
        {
            _gameState = state;

            if (defaultFocusText == null)
            {
                defaultFocusText = state.UserShip.GetActiveTextTag();
                SetDefaultText();
            }
        }
    }


}