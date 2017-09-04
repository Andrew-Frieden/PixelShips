using UnityEngine;
using System.Collections;
using PixelSpace.Models.SharedModels.Ships;
using System;
using System.Collections.Generic;

namespace PixelShips.Verse
{
    public interface IGameState
    {
        Ship PlayerShip { get; }
        IEnumerable<GameStateNotification> Notifications { get; }
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

        public GameState(Ship ship, IEnumerable<GameStateNotification> notifications)
        {
            _playerShip = ship;
            _notifications = notifications;
        }
    }

    public class GameStateNotification
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string SourceId { get; set; }
    }
}