using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ABDialogueController : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private TextMeshProUGUI OptionAText;
    [SerializeField] private TextMeshProUGUI OptionBText;

    [SerializeField] private RectTransform OptionATransform;
    [SerializeField] private RectTransform OptionBTransform;
    [SerializeField] private RectTransform CenterTransform;

    private TextTyper textTyper;

    void Start()
    {
        textTyper = GetComponentInChildren<TextTyper>();
        textTyper.TypeText();
    }

    //  push the AB Control in front of the camera with the given content
    public void ShowControl(ABDialogueContent content, ABDialogueMode mode)
    {
        //  populate the control
        //  hide the text
        textTyper.HideText();
        //  animate the control into view
        //  animate the display of content
        textTyper.TypeText();
        //  wait for user input
    }

    //  hide the control from view. this gets called by the 'back' option.
    public void DismissControl()
    {
    }
    
    //  reset any text or options set from previous content on the control
    private void ClearContent()
    {

    }

    //  populate the text and options on the control with the given content
    private void PopulateControl(ABDialogueContent content)
    {

    }

    private void ShowOptionA()
    {

    }

    private void ShowOptionB()
    {

    }

    private void ShowNoOption()
    {

    }
}
