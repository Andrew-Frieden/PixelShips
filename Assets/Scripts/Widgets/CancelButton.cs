using PixelShips.Widgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelButton : MonoBehaviour {

    public FocusTextWidget Focus;

    public void Cancel()
    {
        Focus.SetDefaultText();
    }
}
