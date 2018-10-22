using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Widgets.Scroller;
using System.Collections.Generic;
using Models.Actions;
using EnumerableExtensions;
using TextSpace.Events;
using System.Collections;

public class ScrollCell : MonoBehaviour, IPointerClickHandler
{
    public bool DisableTouchEvents { get; set; }

    private const int Spacing = 5;
    private bool dimmed;
    private const float DimScale = 0.55f;
    private static float _inverseDimScale => (1.0f / DimScale);
    
    public RectTransform RectTransform;
    [SerializeField] private TextMeshProUGUI EncodedText;
    [SerializeField] private ScrollCellTextTyper Typer;
    private TagString tagString;

    public delegate void LinkTouchedEvent(string guid);
    public static event LinkTouchedEvent linkTouchedEvent;

    private void Start()
    {
        RectTransform = GetComponent<RectTransform>();
        EncodedText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public float SetupScrollCell(TagString encodedCellData, bool start)
    {
        DisableTouchEvents = false;
        tagString = encodedCellData;
        EncodedText.text = tagString.Text;
        Typer.HideText();
        if (start)
        {
            OnCellStarted();
            Typer.TypeText(0.1f); 
        }

        return EncodedText.GetPreferredValues().y + Spacing;
    }

    public void OnCellStarted()
    {
        tagString.Tags.ForEach(t => UIResponseBroadcaster.Broadcast(t));
    }

    public float CellHeight => EncodedText.GetPreferredValues().y + Spacing;

    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (DisableTouchEvents)
            return;

        var gridOffsets = new List<Vector2>()
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(0, 1),
            new Vector2(0, -1),
            new Vector2(1, 1),
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(-1, 1)
        };

        //  calculate a distance that is 2% of the screen width.
        var distance = 2f * 0.01f * (float)UIManager.Instance.UICamera.scaledPixelWidth;

        var origin = eventData.position;
        var positionsToTest = new List<Vector2>();

        foreach (var pos in gridOffsets)
        {
            positionsToTest.Add(origin + (distance * pos));
            positionsToTest.Add(origin + (2f * distance * pos));
        }

        int result;
        foreach (var pos in positionsToTest)
        {
            result = TMP_TextUtilities.FindIntersectingLink(EncodedText, pos, UIManager.Instance.UICamera);
            //Debug.Log($"OnPointerClick -> r:{result} || pos:{pos} || int:{i}");
            if (result >= 0)
            {
                linkTouchedEvent?.Invoke(EncodedText.textInfo.linkInfo[result].GetLinkID());
                break;
            }
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