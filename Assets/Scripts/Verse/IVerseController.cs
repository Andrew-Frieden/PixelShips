using UnityEngine;
using System.Collections;
using PixelSpace.Models.SharedModels;
using PixelSpace.Models.SharedModels.Ships;
using System;

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
    
    public interface IGameState
    {
        DateTime Time { get; }
        Ship PlayerShip { get; }
        ShipState shipState { get; }
    }

    public static class GameState
    {
        public static IGameState Current;
    }

    public class SimpleGameState : IGameState
    {
        public DateTime Time { get; set; }
        public Ship PlayerShip { get; set; }
        public ShipState shipState { get; set; }
    }
}