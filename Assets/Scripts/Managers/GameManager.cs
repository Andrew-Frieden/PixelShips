using UnityEngine;
using Common;
using Events;

public class GameManager : Singleton<GameManager>
{
	public enum GameState
	{
		BOOT,
		PREGAME,
		MISSION
	}

	public EventGameState OnGameStateChanged;

	private GameState _currentGameState = GameState.BOOT;
	public GameState CurrentGameState => _currentGameState;

	private void Start()
	{
		StartGame();
	}

	private void UpdateState(GameState state)
	{
		var previousGameState = _currentGameState;
		_currentGameState = state;

		switch (_currentGameState)
		{
			case GameState.PREGAME:
				break;
			case GameState.MISSION:
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

	private void StartGame()
	{
		UpdateState(GameState.PREGAME);
	}
	
	public void StartMission()
	{
		UpdateState(GameState.MISSION);
	}
}
