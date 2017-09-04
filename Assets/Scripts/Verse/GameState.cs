using UnityEngine;
using System.Collections;
using PixelSpace.Models.SharedModels.Ships;
using System;

namespace PixelShips.Verse
{
    public interface IGameState
    {
        Ship PlayerShip { get; }  
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
        
        public GameState(Ship ship)
        {
            _playerShip = ship;
        }
    }
}