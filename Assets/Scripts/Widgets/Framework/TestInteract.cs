using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using PixelSpace.Models.SharedModels.Helpers;
using PixelShips.Widgets;

public class TestInteract : MonoBehaviour, IPointerClickHandler {

    private TextMeshProUGUI _text;
    public FocusTextWidget FocusText;

	// Use this for initialization
	void Start () {
        _text = GetComponentInChildren<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        var result = TMP_TextUtilities.FindIntersectingLink(_text, eventData.pressPosition, eventData.pressEventCamera);
        if (result >= 0)
        {
            var objectId = _text.textInfo.linkInfo[result].GetLinkID();
            FocusText.SetSelectedObjectId(objectId);
        }
    }

    public void Cancel()
    {
        FocusText.SetDefaultText();
    }
}
