using UnityEngine;
using System.Collections;
using TMPro;
using PixelShips.Verse;

namespace PixelShips.Widgets
{
    public class ExampleWidget : BaseWidget
    {
        public TextMeshProUGUI TextFeed;
    
        protected override void OnVerseUpdate(IGameState state)
        {
            if (TextFeed != null)
            {
                foreach (var note in state.Notifications)
                {
                    TextFeed.text += note.Text + System.Environment.NewLine;
                }
            }
        }

        new void OnEnable()
        {
            base.OnEnable();
            //  any ExampleWidget OnEnable behavior needs to call BaseWidget's OnEnable
        }

        new void OnDisable()
        {
            base.OnDisable();
            //  any ExampleWidget OnDisable behavior needs to call BaseWidget's OnDisable
        }
    }
}