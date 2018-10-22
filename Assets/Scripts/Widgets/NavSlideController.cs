using Models.Actions;

public class NavSlideController : SlideResponseController {

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
