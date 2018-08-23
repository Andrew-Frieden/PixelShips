﻿using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ScrollCell : MonoBehaviour, IPointerClickHandler
{
    private const int Spacing = 5;
    
    private ITextEntity TextEntity { get; set; }
    
    public RectTransform RectTransform;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private TextTyper Typer;

    public delegate void LinkTouchedEvent(ITextEntity entity);
    public static event LinkTouchedEvent linkTouchedEvent;

    private void Start()
    {
        RectTransform = GetComponent<RectTransform>();
        Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public float SetupScrollCell(ITextEntity entity, bool start)
    {
        TextEntity = entity;
        Text.text = entity.GetLookText();
        Typer.HideText();
        if (start)
        {
            Typer.TypeText(0.1f); 
        }

        return Text.GetPreferredValues().y + Spacing;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        var result = TMP_TextUtilities.FindIntersectingLink(Text, eventData.position, UIManager.Instance.UICamera);
        if (result >= 0)
        {
            linkTouchedEvent?.Invoke(TextEntity);
        }
    }
}