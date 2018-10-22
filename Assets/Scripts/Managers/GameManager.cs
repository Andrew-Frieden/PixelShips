using UnityEngine;
using Common;
using Controller;
using Events;
using Models;
using GameData;
using Models.Factories;
using System.Collections.Generic;

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

    //  TODO refactor this hacky thing. should be event driven?
    [SerializeField] private CommandViewController _commandViewController;
	[SerializeField] private BaseViewController _baseViewController;

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

    [SerializeField] List<GameObject> StartupDependencies = new List<GameObject>();
    private List<GameObject> Registrations = new List<GameObject>();

    public void RegisterStartup(GameObject obj)
    {
        Registrations.Add(obj);

        var registrationComplete = true;
        foreach (var dependency in StartupDependencies)
        {
            if (!Registrations.Contains(dependency))
            {
                registrationComplete = false;
            }
        }

        if (registrationComplete)
            _commandViewController.BootstrapView();
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

		var gameContentDto = new ContentLoadController().Load();
		
		RoomFactory = new RoomFactory(gameContentDto);
		ShipFactory = new ShipFactory();
		
		_baseViewController.InitContentLoadResults(gameContentDto);
			
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

    public void StartFromSave()
    {
        GameState = _saveLoadController.Load();
        UpdateState(GamePhase.MISSION);

        //  TODO refactor this hacky thing. Should be event driven probs?
        _commandViewController.StartCommandView();
    }

    public void StartNewMission()
	{
        GameState = _saveLoadController.CreateBootstrapGameState();
        UpdateState(GamePhase.MISSION);
        //_commandViewController.StartCommandView();
        //_commandViewController.BootstrapView();
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
