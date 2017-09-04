using UnityEngine;
using System.Collections;
using TMPro;
using PixelShips.Verse;

namespace PixelShips.Widgets
{
    public class ExampleWidget : BaseWidget
    {    
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void OnVerseUpdate(IGameState state)
        {
            Debug.Log("ExampleWidget -> received verse update");
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