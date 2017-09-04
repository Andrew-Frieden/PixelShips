using UnityEngine;
using System.Collections;
using System;
using PixelShips.Verse;

namespace PixelShips.Widgets
{
    public abstract class BaseWidget : MonoBehaviour
    {
        protected IVerseController verseCtrl
        {
            get { return VerseManager.instance.Ctrl; }
        }

        protected void OnEnable()
        {   
            verseCtrl.Subscribe(OnVerseUpdate);
        }
        
        protected void OnDisable()
        {
            verseCtrl.Unsubscribe(OnVerseUpdate);
        }

        protected virtual void OnVerseUpdate(string data)
        {
            throw new NotImplementedException();
        }
    }
}