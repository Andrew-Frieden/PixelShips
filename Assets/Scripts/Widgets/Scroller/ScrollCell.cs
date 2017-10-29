using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollCell : MonoBehaviour
{
    public RectTransform RectTransform;
    private TextMeshProUGUI Text;

    void Start()
    {
        this.RectTransform = GetComponent<RectTransform>();
        Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetupEmptyCell()
    {
        // reset text
    }
    
    public void RecycleCell()
    {
        //  empty stuff / make inactive?
    }
    
    public void SetDisplay(string text)
    {
        //   set some display stuff
        Text.text = text;
    }
}