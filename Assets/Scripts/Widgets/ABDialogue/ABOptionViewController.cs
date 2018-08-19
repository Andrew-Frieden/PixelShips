using System;
using UnityEngine;

public class ABOptionViewController : TripleViewController {

    [SerializeField] private FixedJoystick joystick;

    [SerializeField] private GameObject CenterView;
    [SerializeField] private GameObject OptionAView;
    [SerializeField] private GameObject OptionBView;

    protected new void Start()
    {
        base.Start();

        if (joystick != null)
        {
            joystick.Dragged += HandleSelection;
        }
    }

    private void HandleSelection(JoyEdge edge)
    {
        switch (edge)
        {
            case JoyEdge.None:
                ShowView(CenterView);
                break;
            case JoyEdge.Center:
                ShowView(CenterView);
                break;
            case JoyEdge.Left:
                ShowView(OptionAView);
                break;
            case JoyEdge.Right:
                ShowView(OptionBView);
                break;
            case JoyEdge.Up:
                break;
            case JoyEdge.Down:
                break;
            default:
                ShowView(CenterView);
                break;
        }
    }
}
