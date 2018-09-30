using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryTripleViewController : TripleViewController 
{
    private delegate void ShowPrimaryView(int view);
    private static event ShowPrimaryView ShowPrimaryViewEvent;

    public static void ShowPrimary(int view)
    {
        ShowPrimaryViewEvent.Invoke(view);
    }

    private new void Start()
    {
        base.Start();
        ShowPrimaryViewEvent += ShowView;
    }

    private void ShowView(int view)
    {
        ShowView(Views[view]);
    }
}