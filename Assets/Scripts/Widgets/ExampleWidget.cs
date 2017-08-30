using UnityEngine;
using System.Collections;

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

        protected override void OnVerseUpdate(string data)
        {
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