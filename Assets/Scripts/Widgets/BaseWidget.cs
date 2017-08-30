using UnityEngine;
using System.Collections;
using System;
using PixelShips.Verse;

namespace PixelShips.Widgets
{
    public abstract class BaseWidget : MonoBehaviour
    {
		protected IVerseController ctrl
		{
			get { return VerseManager.instance.Ctrl; }
		}

		protected void OnEnable()
		{
			ctrl.Subscribe(OnVerseUpdate);
		}

		protected void OnDisable()
		{
			ctrl.Unsubscribe(OnVerseUpdate);
		}

        protected virtual void OnVerseUpdate(string data)
        {
            throw new NotImplementedException();
        }
    }
}