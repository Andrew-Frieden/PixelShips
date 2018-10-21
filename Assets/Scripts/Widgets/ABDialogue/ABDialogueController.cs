using Models.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ABDialogueController : MonoBehaviour {

    [SerializeField] private float SmoothFactor;

    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private TextMeshProUGUI OptionAText;
    [SerializeField] private TextMeshProUGUI OptionBText;
    
    [SerializeField] private Image OptionAViewImage;
    [SerializeField] private Image OptionBViewImage;

    private ABDialogueMode mode;

    private IRoomAction _actionA;
    private IRoomAction _actionB;

    [SerializeField] private Canvas canvas;

    private float screenHeight;
    private TextTyper textTyper;
    private FixedJoystick controlJoystick;

    private Vector2 OnScreen;
    private Vector2 OffScreen;
    private Vector2 TargetLocation;

    private RectTransform ControlRectTransform;
    
    public delegate void OnRoomActionSelect(IRoomAction action);
    public static event OnRoomActionSelect onRoomActionSelect;

    public delegate void ABDialogueModeSet(ABDialogueMode mode);
    public static event ABDialogueModeSet onModeSet;

    void Start()    
    {
        ControlRectTransform = GetComponent<RectTransform>();
        OnScreen = ControlRectTransform.anchoredPosition;

        screenHeight = ControlRectTransform.rect.height * canvas.scaleFactor;
        OffScreen = ControlRectTransform.anchoredPosition + new Vector2(0, screenHeight);

        TargetLocation = OffScreen;
        ControlRectTransform.anchoredPosition = TargetLocation;

        textTyper = GetComponentInChildren<TextTyper>();
        controlJoystick = GetComponentInChildren<FixedJoystick>();

        controlJoystick.Dragged += HandleSelection;
    }

    private JoyEdge previousEdge = JoyEdge.None;

    private void HandleSelection(JoyEdge edge)
    {
        //  anytime we see a joyedge that isn't None, just keep track of it
        if (edge != JoyEdge.None)
        {
            previousEdge = edge;
            return;
        }

        //  don't do anything if the previous and current joyedge is None
        if (previousEdge == JoyEdge.None)
            return;

        //  once you are here, edge = JoyEdge.None and previousEdge represents that last 'selected' edge
        
        switch (previousEdge)
        {
            case JoyEdge.Center:
                break;
            case JoyEdge.Left:
                if (mode == ABDialogueMode.ABCancel || mode == ABDialogueMode.ACancel && _actionA.IsValid)
                {
                    onRoomActionSelect?.Invoke(_actionA);
                    DismissControl();
                }
                break;
            case JoyEdge.Right:
                if (mode == ABDialogueMode.ABCancel && _actionB.IsValid)
                {
                    onRoomActionSelect?.Invoke(_actionB);
                    DismissControl();
                }
                break;
            case JoyEdge.Up:
                break;
            case JoyEdge.Down:
                DismissControl();
                break;
        }
    }

    void Update()
    {
        ControlRectTransform.anchoredPosition = Vector2.Lerp(ControlRectTransform.anchoredPosition, TargetLocation, Time.deltaTime*SmoothFactor);
    }

    //  push the AB Control in front of the camera with the given content
    public void ShowControl(ABDialogueContent content)
    {
        //  populate the control
        PopulateControl(content);
        //  hide the text
        textTyper.HideText();
        //  animate the control into view
        TargetLocation = OnScreen;
        //  animate the display of content
        textTyper.TypeText(0.2f);
        //  wait for user input
        
        SetABBackgrounds(content);
    }

    //  hide the control from view. this gets called by the 'back' option.
    public void DismissControl()
    {
        TargetLocation = OffScreen;
    }

    private void SetABBackgrounds(ABDialogueContent content)
    {
        if (content == null)
            return;

        if (content.OptionAAction != null)
        {
            OptionAViewImage.color = !content.OptionAAction.IsValid 
                ? new Color(150/255f, 150/255f, 150/255f, 48/255f) 
                : new Color(100/255f, 200/255f, 250/255f, 48/255f);
        }

        if (content.OptionBAction == null) 
            return;

        OptionBViewImage.color = content.OptionBAction != null && !content.OptionBAction.IsValid
            ? new Color(150 / 255f, 150 / 255f, 150 / 255f, 48 / 255f)
            : new Color(214 / 255f, 130 / 255f, 124 / 255f, 48 / 255f);
    }
    
    //  populate the text and options on the control with the given content
    private void PopulateControl(ABDialogueContent content)
    {
        DialogueText.text = content.MainText;
        OptionAText.text = content.OptionAText;
        OptionBText.text = content.OptionBText;
        _actionA = content.OptionAAction;
        _actionB = content.OptionBAction;
        mode = content.Mode;
        onModeSet(mode);
    }
}
