using UnityEngine;
using System.Collections;
using System;

namespace PixelShips.Verse
{
    public class VerseManager : MonoBehaviour
    {
        public MockVerseController _ctrl;
        public IVerseController Ctrl
        {
            get
            {
                //if (_ctrl == null)
                    //throw new UnityException("No VerseController connected to VerseManager");
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
                Debug.Log("manager awake " + _ctrl);
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