using UnityEngine;
using System.Collections;
using TMPro;
using PixelShips.Verse;

namespace PixelShips.Widgets
{
    public class ExampleWidget : BaseWidget
    {
        public TextMeshProUGUI TextFeed;
    
    
        // Use this for initialization
        new void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void OnVerseUpdate(IGameState state)
        {
            Debug.Log("ExampleWidget -> received verse update");

            if (TextFeed != null)
            {
                foreach (var note in state.Notifications)
                {
                    TextFeed.text += note.Text + System.Environment.NewLine;
                }
            }
            //  update UI with something from data
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