﻿using PixelSpace.Models.SharedModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using PixelSpace.Models.SharedModels.Ships;

namespace PixelShips.Verse {
    
    public class MockVerseController : MonoBehaviour, IVerseController
    {
        private event VerseUpdate OnUpdate;

        public void StartUpdates()
        {
           Debug.Log("MockVerseController -> starting updates...");
           InvokeRepeating("mockUpdate", 1, 1);
        }
        
        private IGameState GetMockGameState()
        {
            var mockShip = new Ship();
            mockShip.Name = "Pirate Joe Swanson";
            mockShip.MaxHull = 200;
            mockShip.Hull = UnityEngine.Random.Range(25, 190);
            var mockState = new GameState(mockShip);
            return mockState;            
        }
        
        private void mockUpdate() 
        {
            OnUpdate(GetMockGameState());
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

