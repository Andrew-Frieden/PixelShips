using UnityEngine;
using Common;
using Events;
using Models;
using GameData;

public class GameManager : Singleton<GameManager>
{
	public enum GamePhase
	{
		BOOT,
		PREGAME,
		MISSION
	}

	public EventGameState OnGameStateChanged;

	private SaveLoadController _saveLoadController;
	
	private GamePhase _currentGamePhase = GamePhase.BOOT;
	public GamePhase CurrentGamePhase => _currentGamePhase;
	
	public GameState GameState { get; private set; }

	private void Start()
	{
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        StartGame();
	}

	private void UpdateState(GamePhase phase)
	{
		var previousGameState = _currentGamePhase;
		_currentGamePhase = phase;

		switch (_currentGamePhase)
		{
			case GamePhase.PREGAME:
				break;
			case GamePhase.MISSION:
				break;
			default:
				Debug.Log("Invalid Game State");
				break;
		}
		
		OnGameStateChanged.Invoke(_currentGamePhase, previousGameState);
		
		//To listen to this
		//GameManager.Instance.OnGameStateChanged.AddListener(MyGameStateChangedHandler);
		//void MyGameStateChangedHandler(GameManager.GameState currentState, GmaeManager.GameState previous) { ... }
	}

	private void StartGame()
	{
		UpdateState(GamePhase.PREGAME);
		
		_saveLoadController = new SaveLoadController();
	}
	
	public void StartNewMission()
	{
		UpdateState(GamePhase.MISSION);

        //if (Player has a save file)
        //GameState = _saveLoadController.Load();

        //var data = InjectableGameData.SometimesDamageHazards;
        //_saveLoadController.SerializeContent(data);
        
		GameState = _saveLoadController.CreateNewGameState();
	}
}
