using Models.Actions;
using TextSpace.Events;
using UnityEngine;

public class NavBarController : MonoBehaviour {

    private Vector2 OnScreen;
    private Vector2 OffScreen;
    private Vector2 targetPosition;
    private float screenHeight;

    [SerializeField] private float SmoothFactor;
    [SerializeField] private Canvas canvas;

    private RectTransform _rectBase;
    private RectTransform RectBase
    {
        get
        {
            if (_rectBase == null)
                _rectBase = GetComponent<RectTransform>();
            return _rectBase;
        }
    }

    // Use this for initialization
    void Start () {
        screenHeight = RectBase.rect.height * canvas.scaleFactor;
        OnScreen = RectBase.anchoredPosition;
        targetPosition = OnScreen;
        OffScreen = RectBase.anchoredPosition - new Vector2(0, screenHeight);

        UIResponseBroadcaster.UIResponseTagTrigger += RespondToUITag;
    }

    void Update()
    {
        RectBase.anchoredPosition = Vector3.Lerp(RectBase.anchoredPosition, targetPosition, Time.deltaTime * SmoothFactor);
    }

    private void RespondToUITag(UIResponseTag tag)
    {
        if (tag == UIResponseTag.ShowNavBar)
        {
            //  slide it nav bar on
            targetPosition = OnScreen;
        }
        else if (tag == UIResponseTag.HideNavBar)
        {
            //  immediately hide nav bar
            targetPosition = OffScreen;
            RectBase.anchoredPosition = targetPosition;
        }
    }
}
