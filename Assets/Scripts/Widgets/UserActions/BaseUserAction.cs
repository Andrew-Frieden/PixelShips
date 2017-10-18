using PixelShips.Verse;
using PixelSpace.Models.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Widgets.UserActions
{
    public abstract class BaseUserAction
    {
        public abstract void DoTheAction();
    }

    public class MapUserAction : BaseUserAction
    {
        public override void DoTheAction()
        {
            //  show sector links on active text widget
            throw new NotImplementedException();
        }
    }

    public class DismissUserAction : BaseUserAction
    {
        public override void DoTheAction()
        {
            //  drop focus on whatever was currently selected, if any
            throw new NotImplementedException();
        }
    }

    public class UserSpaceAction : BaseUserAction
    {
        public SpaceAction Action { get; set; }
        public IVerseController Controller { get; set; }

        public override void DoTheAction()
        {
            
            Controller.SubmitAction(Action);
        }
    }
}
