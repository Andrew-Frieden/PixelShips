using UnityEngine;
using System.Collections;
using System;
using PixelShips.Verse;

namespace PixelShips.Widgets
{
    public abstract class BaseWidget : MonoBehaviour
    {
        private bool _subscribed;
    
        protected IVerseController verseCtrl { get { return VerseManager.instance.Ctrl; } }

        protected void OnEnable()
        {
            if (_subscribed)
                throw new UnityException("BaseWidget -> already subscribed before being enabled!");

            verseCtrl.Subscribe(OnVerseUpdate);
            _subscribed = true;
        }

        protected void OnDisable()
        {
            if (!_subscribed)
                throw new UnityException("BaseWidget -> already unsubscribed before being disabled!");

            verseCtrl.Unsubscribe(OnVerseUpdate);
            _subscribed = false;
        }

        protected virtual void OnVerseUpdate(IGameState state)
        {
            throw new NotImplementedException(string.Format("{0} is missing OnVerseUpdate implementation!", this.GetType().Name));
        }
    }
}