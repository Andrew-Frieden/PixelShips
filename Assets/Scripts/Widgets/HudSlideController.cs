using Models.Actions;

public class HudSlideController : SlideResponseController {

    protected override void RespondToUITag(UIResponseTag tag)
    {
        if (tag == UIResponseTag.ShowHUD)
        {
            //  slide it nav bar on
            TargetPosition = OnScreen;
        }
        else if (tag == UIResponseTag.HideHUD)
        {
            //  immediately hide nav bar
            TargetPosition = OffScreen;
            RectBase.anchoredPosition = TargetPosition;
        }
    }
}
