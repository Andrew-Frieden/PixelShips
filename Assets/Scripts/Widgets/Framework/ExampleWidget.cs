using UnityEngine;
using PixelShips.Verse;

namespace PixelShips.Widgets
{
    public class ExampleWidget : BaseWidget
    {
        public GameObject SomePrefabInstance;

        protected override void OnVerseUpdate(IGameState state)
        {
            //  respond to latest state update
            //  SomePrefabInstance.GetComponent<YourFancyScript>.DoTheThing(state.Ships);
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