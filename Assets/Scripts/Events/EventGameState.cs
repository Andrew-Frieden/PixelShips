using UnityEngine.Events;

namespace Events
{
    [System.Serializable]
    public class EventGameState : UnityEvent<GameManager.GamePhase, GameManager.GamePhase> { }
}