using UnityEngine;
using Common;
using Events;

public class GameManager : Singleton<GameManager>
{
	public enum GameState
	{
		PREGAME,
		MISSION,
		ENCOUNTER,
		SPACESTATION
	}

	public EventGameState OnGameStateChanged;

	private GameState _currentGameState = GameState.PREGAME;
	public GameState CurrentGameState => _currentGameState;

	void UpdateState(GameState state)
	{
		var previousGameState = _currentGameState;
		_currentGameState = state;

		switch (_currentGameState)
		{
			case GameState.PREGAME:
				break;
			case GameState.MISSION:
				break;
			case GameState.ENCOUNTER:
				break;
			case GameState.SPACESTATION:
				break;
			default:
				Debug.Log("Invalid Game State");
				break;
		}
		
		OnGameStateChanged.Invoke(_currentGameState, previousGameState);
		
		//To listen to this
		//GameManager.Instance.OnGameStateChanged.AddListener(MyGameStateChangedHandler);
		//void MyGameStateChangedHandler(GameManager.GameState currentState, GmaeManager.GameState previous) { ... }
	}
}
