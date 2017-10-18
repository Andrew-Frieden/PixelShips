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

    public interface IGameState : ISpaceState
    {
        DateTime UserTime { get; }
        Ship UserShip { get; }
        Room UserRoom { get; }
        IEnumerable<FeedUpdate> Notifications { get;  }
        IEnumerable<SpaceAction> UserActions { get; }
    }

    public class SimpleGameState : IGameState
    {
        public Ship UserShip { get; private set; }
        public Room UserRoom { get; private set; }

        public DateTime UserTime { get; private set; }

        public IEnumerable<FeedUpdate> Notifications { get; private set; }
        public IEnumerable<SpaceAction> UserActions { get; private set; }

        public IEnumerable<Room> Rooms { get; set; }
        public IEnumerable<Ship> Ships { get; set; }

        public SimpleGameState(ShipState state)
        {
            UserTime = DateTime.UtcNow;
            var rs = new RoomState(state.Room, state.Room.Ships);
            UserShip = state.Ship;
            UserRoom = rs.Room;
            Ships = rs.Ships;
            Rooms = rs.Rooms;
            Notifications = UserRoom.Notifications;
            var actions = new List<SpaceAction>();
            var factory = new SpaceActionFactory(rs);
            state.PossibleActions.ForEach(dbi => actions.Add(factory.GetModel(dbi)));
            UserActions = actions;
        }
    }
}