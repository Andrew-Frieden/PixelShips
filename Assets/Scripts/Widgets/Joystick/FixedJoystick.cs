using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FixedJoystick : Joystick
{
    public delegate void DraggedToEdge(JoyEdge edge);
    public event DraggedToEdge Dragged;

    [SerializeField] private Camera Camera;

    [SerializeField] private Image BaseImage;
    [SerializeField] private Image HandleImage;

    [SerializeField] private float SelectionThreshold = 0.85f;

    private Vector2 joystickPosition;
    private JoyEdge currentEdge = JoyEdge.None;

    void Start()
    {
    }

    public override void OnDrag(PointerEventData eventData)
    {
        joystickPosition = RectTransformUtility.WorldToScreenPoint(Camera, background.position);

        Vector2 direction = eventData.position - joystickPosition;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;

        if (inputVector.x >= SelectionThreshold)
        {
            SetJoyEdge(JoyEdge.Right);
        }
        else if (inputVector.x <= -SelectionThreshold)
        {
            SetJoyEdge(JoyEdge.Left);
        }
        else if (inputVector.y <= -SelectionThreshold)
        {
            SetJoyEdge(JoyEdge.Down);
        }
        else if (inputVector.y >= SelectionThreshold)
        {
            SetJoyEdge(JoyEdge.Up);
        }
        else
        {
            SetJoyEdge(JoyEdge.Center);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        SetJoyEdge(JoyEdge.None);
    }

    private void SetJoyEdge(JoyEdge edge)
    {
        if (currentEdge != edge)
        {
            currentEdge = edge;
            Dragged(currentEdge);
        }
    }
}

public enum JoyEdge
{
    None,
    Center,
    Left,
    Right,
    Up,
    Down
}