using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//  script that listens to player input and causes the control to react
public class ABControlReactor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Cancel;

    [SerializeField] private Image JoystickHandleImage;
    [SerializeField] private Image JoystickBackgroundImage;
    [SerializeField] private Image LeftImage;
    [SerializeField] private Image RightImage;

    [SerializeField] private Color LeftColorRest;
    [SerializeField] private Color LeftColorSelected;
    [SerializeField] private Color RightColorRest;
    [SerializeField] private Color RightColorSelected;
    [SerializeField] private Color JoystickDefaultColor;
    [SerializeField] private Color CancelDefaultColor;
    [SerializeField] private Color CancelSelectedColor;

    [SerializeField] private float ImageSelectionScale;

    //  optional: hide the joystick handle when no touch is detected, hide the joystick base when touch is detected.
    [SerializeField] private bool Disjointed_Display;

    [SerializeField] private FixedJoystick joyStick;

    void Start()
    {
        if (joyStick == null)
        {
            joyStick = GetComponentInChildren<FixedJoystick>();
        }
        joyStick.Dragged += HandleSelection;

        ABDialogueController.onModeSet += HandleMode;

        SetDefaults();
        ShowJoystickBackground();
    }

    void SetDefaults()
    {
        LeftImage.color = LeftColorRest;
        RightImage.color = RightColorRest;
        JoystickHandleImage.color = JoystickDefaultColor;
        JoystickBackgroundImage.color = JoystickDefaultColor;
        LeftImage.rectTransform.localScale = Vector3.one;
        RightImage.rectTransform.localScale = Vector3.one;
        Cancel.rectTransform.localScale = Vector3.one;
        Cancel.faceColor = CancelDefaultColor;
    }

    private void HideImage(Image image)
    {
        image.CrossFadeAlpha(0f, 0.0f, false);
    }

    private void ShowImage(Image image)
    {
        image.CrossFadeAlpha(255f, 0.0f, false);
    }

    private void ShowJoystickBackground()
    {
        if (Disjointed_Display)
        {
            HideImage(JoystickHandleImage);
        }
        else
        {
            ShowImage(JoystickHandleImage);
        }
        ShowImage(JoystickBackgroundImage);
    }

    private void ShowJoystickHandle()
    {
        if (Disjointed_Display)
        {
            HideImage(JoystickBackgroundImage);
        }
        else
        {
            ShowImage(JoystickBackgroundImage);
        }
        ShowImage(JoystickHandleImage);
    }

    private void HandleMode(ABDialogueMode mode)
    {
        switch (mode)
        {
            case ABDialogueMode.ABCancel:
                Cancel.enabled = true;
                LeftImage.enabled = true;
                RightImage.enabled = true;
                break;
            case ABDialogueMode.ACancel:
                Cancel.enabled = true;
                LeftImage.enabled = true;
                RightImage.enabled = false;
                break;
            case ABDialogueMode.Cancel:
                Cancel.enabled = true;
                LeftImage.enabled = false;
                RightImage.enabled = false;
                break;
            default:
                throw new Exception($"ABControlReactor.HandleMode() unsupported mode: {mode}");
        }
    }

    private void HandleSelection(JoyEdge edge)
    {
        switch (edge)
        {
            case JoyEdge.None:
                SetDefaults();
                ShowJoystickBackground();
                break;
            case JoyEdge.Center:
                SetDefaults();
                ShowJoystickHandle();
                break;
            case JoyEdge.Left:
                LeftImage.color = LeftColorSelected;
                JoystickHandleImage.color = LeftColorSelected;
                LeftImage.rectTransform.localScale = new Vector3(ImageSelectionScale, ImageSelectionScale);
                break;
            case JoyEdge.Right:
                RightImage.color = RightColorSelected;
                JoystickHandleImage.color = RightColorSelected;
                RightImage.rectTransform.localScale = new Vector3(ImageSelectionScale, ImageSelectionScale);
                break;
            case JoyEdge.Up:
                break;
            case JoyEdge.Down:
                Cancel.faceColor = CancelSelectedColor;
                Cancel.rectTransform.localScale = new Vector3(ImageSelectionScale, ImageSelectionScale);
                break;
            default:
                break;
        }
    }
}
