using PixelSpace.Models.SharedModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using PixelSpace.Models.SharedModels.Ships;
using PixelSpace.Models.SharedModels.SpaceUpdates;

namespace PixelShips.Verse {
    
    public class MockVerseController : MonoBehaviour, IVerseController
    {
        private event VerseUpdate OnUpdate;

        public void StartUpdates()
        {
           Debug.Log("MockVerseController -> starting updates...");
           InvokeRepeating("mockUpdate", 1, 3);
        }
        
        private IGameState GetMockGameState()
        {
            var mockShip = new Ship();            
            mockShip.Name = "Pirate Joe Swanson";
            mockShip.MaxHull = 200;
            mockShip.Hull = UnityEngine.Random.Range(25, 190);

            var mockState = new GameState(mockShip, new MockSpaceState(), GetMockNotifications());
            return mockState;            
        }

        private IEnumerable<GameStateNotification> GetMockNotifications()
        {
            var notifications = new List<GameStateNotification>();
            notifications.Add(new GameStateNotification(){ Id = "123", Text = "A <color=purple>[ship]</color> has arrived.", SourceId = "345abcd" });
            notifications.Add(new GameStateNotification(){ Id = "345", Text = "A torpedo was launched at your ship by <color=orange>[Pirate Ship]</color>", SourceId = "345abcd" });
            notifications.Add(new GameStateNotification(){ Id = "678", Text = "A <color=purple>[ship]</color> has begun spinning up its jump drive.", SourceId = "345abcd" });
            notifications.Add(new GameStateNotification(){ Id = "678", Text = "A distant comet streaks across your view.", SourceId = "" });
            notifications.Add(new GameStateNotification(){ Id = "678", Text = "A <color=orange>[Pirate Ship]</color> is warming up weapons systems!", SourceId = "" });
            notifications.Add(new GameStateNotification(){ Id = "678", Text = "A <color=purple>[ship]</color> is idling in the sector", SourceId = "" });

            var note = notifications[UnityEngine.Random.Range(0, notifications.Count)];
            return new List<GameStateNotification>() { note };
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

    public class MockSpaceState : ISpaceState
    {
        public IEnumerable<SpaceAction> Actions
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IDictionary<string, ShipFeed> FeedMap
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<ShipFeed> Feeds
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IDictionary<string, Room> RoomMap
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Room> Rooms
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IDictionary<string, Ship> ShipMap
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Ship> Ships
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

