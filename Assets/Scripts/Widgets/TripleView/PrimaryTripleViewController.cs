using Models.Actions;
using System.Collections;
using System.Collections.Generic;
using TextSpace.Events;

public class PrimaryTripleViewController : TripleViewController 
{
    private new void Start()
    {
        base.Start();
        UIResponseBroadcaster.UIResponseTagTrigger += RespondToUITag;
    }

    private void RespondToUITag(UIResponseTag tag)
    {
        if (tag == UIResponseTag.ViewCmd)
        {
            ShowView(TripleView.Middle);
        }
        else if (tag == UIResponseTag.ViewBase)
        {
            ShowView(TripleView.Left);
        }
    }

    private void ShowView(TripleView view) { ShowView(Views[(int)view]); }
}