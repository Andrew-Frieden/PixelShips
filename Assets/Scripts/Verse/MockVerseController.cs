﻿using PixelSpace.Models.SharedModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelShips.Verse {
    
    public class MockVerseController : IVerseController
    {
        private event VerseUpdate OnUpdate;

        public void StartUpdates()
        {
            OnUpdate("test");

            throw new NotImplementedException();
        }

        public bool SubmitAction(string action)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(VerseUpdate updateDelegate)
        {
            OnUpdate += updateDelegate;
        }

        public void Unsubscribe(VerseUpdate updateDelegate)
        {
            OnUpdate -= updateDelegate;
        }
    }
}

