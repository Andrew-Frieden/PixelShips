using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Widgets.Scroller;

public class ScrollCell : MonoBehaviour, IPointerClickHandler
{
    private const int Spacing = 5;
    
    public RectTransform RectTransform;
    [SerializeField] private TextMeshProUGUI EncodedText;
    [SerializeField] private ScrollCellTextTyper Typer;

    public delegate void LinkTouchedEvent(string guid);
    public static event LinkTouchedEvent linkTouchedEvent;

    private void Start()
    {
        RectTransform = GetComponent<RectTransform>();
        EncodedText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public float SetupScrollCell(string encodedCellText, bool start)
    {
        EncodedText.text = encodedCellText;
        Typer.HideText();
        if (start)
        {
            Typer.TypeText(0.1f); 
        }

        return EncodedText.GetPreferredValues().y + Spacing;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        var result = TMP_TextUtilities.FindIntersectingLink(EncodedText, eventData.position, UIManager.Instance.UICamera);
        if (result >= 0)
        {
            linkTouchedEvent?.Invoke(EncodedText.textInfo.linkInfo[result].GetLinkID());
        }
    }

    public void Dim()
    {
        EncodedText.CrossFadeAlpha(0.5f, 0.5f, true);
    }
}