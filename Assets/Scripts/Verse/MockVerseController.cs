﻿using PixelSpace.Models.SharedModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using PixelSpace.Models.SharedModels.Ships;

namespace PixelShips.Verse {
    
    public class MockVerseController : IVerseController
    {
        private event VerseUpdate OnUpdate;

        private System.Random rng = new System.Random();

        public void StartUpdates()
        {
            Debug.Log("MockVerseController -> starting updates...");
            
           var myTimer = new Timer();
           myTimer.Elapsed += new ElapsedEventHandler(myEvent);
           myTimer.Interval = 1000;
           myTimer.Enabled = true;
        }
        
        private IGameState GetMockGameState()
        {
            var mockShip = new Ship();

            mockShip.Name = "Pirate Joe Swanson";
            mockShip.MaxHull = 200;
            mockShip.Hull = (int)((rng.NextDouble() * 180) + 10); 
                       
            var mockState = new GameState(mockShip);
            return mockState;            
        }
        
        private void myEvent(object source, ElapsedEventArgs e) 
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

