using UnityEngine;
using System.Collections;
using System;
using PixelShips.Verse;

namespace PixelShips.Widgets
{
    public abstract class BaseWidget : MonoBehaviour
    {
        private bool subscribed;
    
        protected IVerseController verseCtrl
        {
            get 
            {
                if (VerseManager.instance == null)
                    return null;
                return VerseManager.instance.Ctrl; 
            }
        }

        protected void Start()
        {
            if (!subscribed)
            {
                subscribed = true;
                verseCtrl.Subscribe(OnVerseUpdate);
            }
        }

        protected void OnEnable()
        {
            if (verseCtrl != null && !subscribed)
            {
                subscribed = true;
                verseCtrl.Subscribe(OnVerseUpdate);
            }
        }
        
        protected void OnDisable()
        {
            if (verseCtrl != null && subscribed)
            {
                verseCtrl.Unsubscribe(OnVerseUpdate);
                subscribed = false;
            }
        }

        protected virtual void OnVerseUpdate(IGameState state)
        {
            throw new NotImplementedException();
        }
    }
}