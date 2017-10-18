using PixelShips.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Widgets.Framework
{
    public class UIManager : BaseWidget
    {

        private void Start()
        {
            widgets = new Dictionary<string, BaseWidget>();
        }
        private Dictionary<string, BaseWidget> widgets;

        public static readonly string MAP_WIDGET_KEY = "map";
        public static readonly string FOCUS_WIDGET_KEY = "focus";
        public static readonly string CANCEL_WIDGET_KEY = "cancel";
        public static readonly string SCAN_WIDGET_KEY = "scan";
        public static readonly string JUMP_WIDGET_KEY = "jump";

        public void SetObjectFocus(string id)
        {
            if (widgets.ContainsKey(FOCUS_WIDGET_KEY))
            {
                var focusWidget = widgets[FOCUS_WIDGET_KEY] as FocusTextWidget;
                focusWidget.SetSelectedObjectId(id);
            }

            if (widgets.ContainsKey(CANCEL_WIDGET_KEY))
            {
            }
        }

        public void DropFocus()
        {

        }

        public void BindWidget(string key, BaseWidget widget)
        {
            if (widgets.ContainsKey(key))
            {
                throw new Exception("UIManager BindWidget() already bound for key: " + key);
            }

            widgets[key] = widget;
        }
    }
}
