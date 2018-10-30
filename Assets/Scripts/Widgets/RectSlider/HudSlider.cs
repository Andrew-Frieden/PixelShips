using TextSpace.Models.Actions;

public class HudSlider : RectSlider {

    new void Start()
    {
        base.Start();
        GameManager.Instance.RegisterStartup(gameObject);
    }

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
