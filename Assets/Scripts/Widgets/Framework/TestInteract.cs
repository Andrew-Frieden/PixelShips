using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using PixelSpace.Models.SharedModels.Helpers;

public class TestInteract : MonoBehaviour, IPointerClickHandler {

    private TextMeshProUGUI _text;

    public TextMeshProUGUI FocusText;

	// Use this for initialization
	void Start () {
        _text = GetComponentInChildren<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer Click");

        //var result = TMP_TextUtilities.FindIntersectingWord(_text, Input.mousePosition, Camera.current);
        // _text.textInfo.linkInfo.ForEach(i => Debug.Log("link txt: " + i.GetLinkText() + " id: " + i.GetLinkID() + " hash: " + i.hashCode));
        var result = TMP_TextUtilities.FindIntersectingLink(_text, Input.mousePosition, Camera.current);
        if (result >= 0)
        {
            var objectId = _text.textInfo.linkInfo[result].GetLinkID();
            FocusText.text = string.Format("[{0}]", objectId);
            //Debug.Log("clicked: " + _text.textInfo.linkInfo[result].GetLinkID());
        }
    }
}
