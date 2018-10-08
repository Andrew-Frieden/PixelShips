using UnityEngine;
using Common;
using Controller;
using Events;
using Models;
using GameData;
using Models.Factories;

public class GameManager : Singleton<GameManager>, ISaveManager
{
	public enum GamePhase
	{
		BOOT,
		PREGAME,
		MISSION
	}

	public EventGameState OnGameStateChanged;

	private SaveLoadController _saveLoadController;
	private ContentLoadController _contentLoadController;
	
	private GamePhase _currentGamePhase = GamePhase.BOOT;
	public GamePhase CurrentGamePhase => _currentGamePhase;
	
	public GameState GameState { get; private set; }

	public static RoomFactory RoomFactory;
	public static ShipFactory ShipFactory;

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
        _saveLoadController = new SaveLoadController();
		_saveLoadController.Init();
		
		RoomFactory = new RoomFactory(new ContentLoadController().Load());
		ShipFactory = new ShipFactory();
			
        UpdateState(GamePhase.PREGAME);
	}

    //  expose some save stuff for the main menu
    public bool HasSaveFile =>_saveLoadController.HasSaveData;
    public SaveState SaveFile => _saveLoadController.SaveData;
    public string SavePath => SaveLoadController.SaveFilePath;
    public void ResetSaveData()
    {
        _saveLoadController.Delete();
    }

    public void StartNewMission()
	{
		UpdateState(GamePhase.MISSION);

        //if (Player has a save file)
        //GameState = _saveLoadController.Load();

		GameState = _saveLoadController.CreateNewGameState();
	}

    void OnApplicationPause(bool pauseStatus)
    {
        if (GameState != null && pauseStatus)
            _saveLoadController.Save(GameState);
    }
}

public interface ISaveManager
{
    bool HasSaveFile { get; }
    SaveState SaveFile { get; }
    string SavePath { get; }
    void ResetSaveData();
}
