using UnityEngine;
using System.Collections;
using PixelSpace.Models.SharedModels;
using PixelSpace.Models.SharedModels.Ships;
using System;
using System.Collections.Generic;
using PixelSpace.Models.SharedModels.SpaceUpdates;
using PixelSpace.Models.SharedModels.SpaceActions;
using PixelSpace.Models.SharedModels.Helpers;

namespace PixelShips.Verse
{
    public delegate void VerseUpdate(IGameState state);

    public interface IVerseController
    {
        void Subscribe(VerseUpdate updateDelegate);
        void Unsubscribe(VerseUpdate updateDelegate);
        void StartUpdates();
        bool SubmitAction(SpaceAction spaceAction);
    }
    
    //public interface IGameState
    //{
    //    DateTime Time { get; }
    //    Ship PlayerShip { get; }
    //    ShipState shipState { get; }
    //}

    public interface IGameState : IRoomState
    {
        Ship UserShip { get; set; }
        DateTime UserTime { get; }
        IEnumerable<FeedUpdate> Notifications { get;  }
        IEnumerable<SpaceAction> UserActions { get; }
    }

    public class SimpleGameState : IGameState
    {
        public DateTime UserTime { get; private set; }

        public IEnumerable<FeedUpdate> Notifications { get; private set; }
        public IEnumerable<SpaceAction> UserActions { get; private set; }

        public IEnumerable<Ship> Ships { get; set; }
        public Room Room { get; set; }
        public Ship UserShip { get; set; }

        public SimpleGameState(ShipState state)
        {
            UserTime = DateTime.UtcNow;
            UserShip = state.Ship;
            Room = state.Room;
            Ships = state.Ships;

            if (Room != null)
            {
                Notifications = state.Room.Notifications;
                var factory = new SpaceActionFactory(state);
                UserActions = factory.GetPossibleActionsForShip(state.Ship);
            }
        }
    }
}