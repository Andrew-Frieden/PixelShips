using TextSpace.Models.Actions;

public class NavSlider : RectSlider {

    new void Start()
    {
        base.Start();
        GameManager.Instance.RegisterStartup(gameObject);
    }

    protected override void RespondToUITag(UIResponseTag tag)
    {
        if (tag == UIResponseTag.ShowNavBar)
        {
            //  slide it nav bar on
            TargetPosition = OnScreen;
        }
        else if (tag == UIResponseTag.HideNavBar)
        {
            //  immediately hide nav bar
            TargetPosition = OffScreen;
            RectBase.anchoredPosition = TargetPosition;
        }
    }
}
