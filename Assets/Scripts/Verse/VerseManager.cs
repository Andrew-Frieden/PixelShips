using UnityEngine;
using System.Collections;
using System;

namespace PixelShips.Verse
{
    public class VerseManager : MonoBehaviour
    {
        private IVerseController _ctrl;
        public IVerseController Ctrl
        {
            get
            {
                if (_ctrl == null)
                    _ctrl = new MockVerseController();
                return _ctrl;
            }
        }

        private static VerseManager _instance;
        public static VerseManager instance
        {
            get
            {
                //if (_instance == null)
                    //throw new Exception("VerseManager not set!");
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
                throw new Exception("VerseManager already found!");
            }
        }

        void Start()
        {
            Ctrl.StartUpdates();
        }
    }
}