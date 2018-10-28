using UnityEngine;
using Common;
using Controller;
using Events;
using Models;
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
	[SerializeField] private HomeViewController _baseViewController;

    private GamePhase _currentGamePhase = GamePhase.BOOT;
	public GamePhase CurrentGamePhase => _currentGamePhase;
	
	public GameState GameState { get; private set; }

	public static RoomFactory RoomFactory;
	public static ShipFactory ShipFactory;

	private void Start()
	{
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        _saveLoadController = new SaveLoadController();
        _saveLoadController.Init();

        var gameContentDto = new ContentLoadController().Load();
        RoomFactory = new RoomFactory(gameContentDto);
        ShipFactory = new ShipFactory();

        _baseViewController.InitContentLoadResults(gameContentDto);

        UpdateState(GamePhase.PREGAME);
    }

    [SerializeField] List<GameObject> StartupDependencies = new List<GameObject>();
    private List<GameObject> Registrations = new List<GameObject>();
    private bool bootstrapping;

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

        if (registrationComplete && bootstrapping)
        {
            bootstrapping = false;
            _commandViewController.BootstrapView();
        }
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

    //  expose some save stuff for the main menu
    public bool HasSaveFile =>_saveLoadController.HasSaveData;
    public SaveState SaveFile => _saveLoadController.SaveData;
    public string SavePath => SaveLoadController.SaveFilePath;
    public void ResetSaveData()
    {
        _saveLoadController.Delete();
    }

    public void ContinueFromSave()
    {
        GameState = _saveLoadController.Load();
        UpdateState(GamePhase.MISSION);

        //  TODO refactor this hacky thing. Should be event driven probs?
        _commandViewController.StartCommandView();
    }

    public void BootstrapNewGame(bool devSettingsEnabled = false)
	{
        bootstrapping = true;
        GameState = _saveLoadController.CreateBootstrapGameState(devSettingsEnabled);
        UpdateState(GamePhase.MISSION);
    }

    public void StartNewExpedition()
    {
        //  this should only start a new expedition, not wipe out the entire gamestate
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
