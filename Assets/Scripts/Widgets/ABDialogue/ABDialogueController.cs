using Models.Actions;
using TMPro;
using UnityEngine;

public class ABDialogueController : MonoBehaviour {

    [SerializeField] private float SmoothFactor;

    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private TextMeshProUGUI OptionAText;
    [SerializeField] private TextMeshProUGUI OptionBText;

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
    
    public delegate void ChoseActionEvent(IRoomAction action);
    public static event ChoseActionEvent choseActionEvent;

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
        if (edge != JoyEdge.None)
        {
            previousEdge = edge;
            return;
        }

        switch (previousEdge)
        {
            case JoyEdge.Center:
                break;
            case JoyEdge.Left:
                choseActionEvent?.Invoke(_actionA);
                DismissControl();
                break;
            case JoyEdge.Right:
                choseActionEvent?.Invoke(_actionA);
                DismissControl();
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
    }

    public void ShowControl()
    {
        var content = new ABDialogueContent()
        {
            MainText = @"A tabby officer greets you over the voice comms:

Meow Citizen!

I was in pursuit of two renegade Verdants in this sectorrr but my ship got stuck in the kelp.Can you renderrr me some assistance ? ",
            OptionAText = "Might want to consider this idea",
            OptionBText = "Sounds risky"
        };

        ShowControl(content);
    }

    //  hide the control from view. this gets called by the 'back' option.
    public void DismissControl()
    {
        TargetLocation = OffScreen;
    }

    //  populate the text and options on the control with the given content
    private void PopulateControl(ABDialogueContent content)
    {
        DialogueText.text = content.MainText;
        OptionAText.text = content.OptionAText;
        OptionBText.text = content.OptionBText;
        _actionA = content.OptionAAction;
        _actionB = content.OptionBAction;
    }
}
