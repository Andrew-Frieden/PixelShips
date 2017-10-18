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
    public class JumpButton : BaseWidget
    {
        public StartInstantJumpAction JumpAction;
        public Assets.Scripts.Verse.SimpleVerseController VerseController;
        public FocusTextWidget FocusWidget;

        protected override void OnVerseUpdate(IGameState state)
        {
        }

        public void StartJump()
        {
            VerseController.SubmitAction(JumpAction);
            FocusWidget.SetDefaultText();
        }
    }
}
