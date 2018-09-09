using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Widgets.Scroller;

public class ScrollCell : MonoBehaviour, IPointerClickHandler
{
    private const int Spacing = 5;
    private bool dimmed;
    private readonly float DimScale = 0.55f;
    private float _inverseDimScale => (1.0f / DimScale);
    
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

    public void Dim(bool dim)
    {
        if (dim == dimmed)
            return;

        if (dim)
        {
            var color = EncodedText.color;
            color.r = color.r * DimScale;
            color.g = color.g * DimScale;
            color.b = color.b * DimScale;
            EncodedText.color = color;
        }
        else
        {
            var color = EncodedText.color;
            color.r = color.r * _inverseDimScale;
            color.g = color.g * _inverseDimScale;
            color.b = color.b * _inverseDimScale;
            EncodedText.color = color;
        }

        dimmed = dim;
    }
}