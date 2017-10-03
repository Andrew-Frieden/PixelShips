using UnityEngine;
using System.Collections;
using System;

namespace PixelShips.Verse
{
    public class VerseManager : MonoBehaviour
    {
        public IVerseController _ctrl;
        public IVerseController Ctrl
        {
            get
            {
                if (_ctrl == null)
                {
                    _ctrl = gameObject.GetComponent<IVerseController>();
                    if (_ctrl == null)
                        throw new UnityException("VerseManager -> No VerseController found!");
                }
                return _ctrl;
            }
        }

        private static VerseManager _instance;
        public static VerseManager instance
        {
            get
            {
                //if (_instance == null)
                //    throw new UnityException("VerseManager -> instance referenced too early in unity lifecycle.");
                return _instance;
            }
        }
        
        void Awake()
        {
            if (_instance == null || _instance == this)
            {
                _instance = this;
            }
            else
            {
                throw new UnityException("VerseManager -> instance already found!");
            }
        }

        void Start()
        {
            Ctrl.StartUpdates();
        }
    }
}