using Models.Actions;
using TextSpace.Events;
using UnityEngine;

public class SlideResponseController : MonoBehaviour {

    protected Vector2 TargetPosition;
    protected Vector2 OnScreen;
    protected Vector2 OffScreen;

    private float screenHeight;

    [SerializeField] private float SmoothFactor = 20f;
    [SerializeField] private Canvas canvas;
    [SerializeField] private bool SlideUp;

    private RectTransform _rectBase;
    protected RectTransform RectBase
    {
        get
        {
            if (_rectBase == null)
                _rectBase = GetComponent<RectTransform>();
            return _rectBase;
        }
    }

    // Use this for initialization
    protected void Start () {
        screenHeight = RectBase.rect.height * canvas.scaleFactor;
        OnScreen = RectBase.anchoredPosition;
        TargetPosition = OnScreen;

        var offset = SlideUp ? new Vector2(0, screenHeight) : new Vector2(0, -screenHeight);
        OffScreen = RectBase.anchoredPosition + offset;

        UIResponseBroadcaster.UIResponseTagTrigger += RespondToUITag;
    }

    void Update()
    {
        RectBase.anchoredPosition = Vector3.Lerp(RectBase.anchoredPosition, TargetPosition, Time.deltaTime * SmoothFactor);
    }

    protected virtual void RespondToUITag(UIResponseTag tag)
    {
    }
}
