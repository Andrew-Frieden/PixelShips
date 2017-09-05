using UnityEngine;
using System.Collections;
using PixelSpace.Models.SharedModels;
using System;
using System.Collections.Generic;
using PixelSpace.Models.SharedModels.Ships;

namespace PixelShips.Verse
{
    public interface IGameState
    {
        Ship PlayerShip { get; }
        IEnumerable<GameStateNotification> Notifications { get; }
        ISpaceState SpaceState { get; }
    }

    public class GameState : IGameState
    {
        private Ship _playerShip;
        public Ship PlayerShip
        {
            get
            {
                return _playerShip;
            }
        }

        private IEnumerable<GameStateNotification> _notifications;
        public IEnumerable<GameStateNotification> Notifications
        {
            get
            {
                return _notifications;
            }
        }

        private ISpaceState _spaceState;
        public ISpaceState SpaceState
        {
            get
            {
                return _spaceState;
            }
        }

        public GameState(Ship ship, ISpaceState spaceState, IEnumerable<GameStateNotification> notifications)
        {
            _playerShip = ship;
            _notifications = notifications;
            _spaceState = spaceState;
        }
    }

    public class GameStateNotification
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string SourceId { get; set; }
    }
}