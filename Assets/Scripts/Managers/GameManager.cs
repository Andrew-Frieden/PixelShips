using UnityEngine;
using Common;
using Controller;
using Events;
using Models;
using Models.Factories;
using System.Collections.Generic;
using TextSpace.Events;
using Models.Actions;

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
	[SerializeField] private HomeViewController _homeViewController;

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

        _homeViewController.InitContentLoadResults(gameContentDto);

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
        _homeViewController.Init(GameState.Home);
    }

    public void BootstrapNewGame(bool devSettingsEnabled = false)
	{
        bootstrapping = true;
        GameState = CreateBootstrapGameState(devSettingsEnabled);
        UpdateState(GamePhase.MISSION);
        _homeViewController.Init(GameState.Home);
    }

    public void StartNewExpedition()
    {
        //  for the quick dev start, replace the bootstrapping world with a placeholder dev one
        if (GameState.Home == BootstrapWorld)
            SetHomeworld(new Homeworld() { PlanetName = "Earth", Description = "Developer" });

        GameState.Home.ExpeditionCount++;
        GameState.CurrentExpedition = new Expedition
        {
            CmdShip = ShipFactory.GenerateCommandShip(RoomFactory),
            Room = (Room)RoomFactory.GenerateRoom(new RoomTemplate(10, RoomFlavor.Kelp)),
            CurrentMission = new Mission { MissionLevel = 1 }
        };
        _commandViewController.StartCommandView();
        UIResponseBroadcaster.Broadcast(UIResponseTag.UpdateHomeworld);
    }

    public void SetHomeworld(Homeworld world)
    {
        GameState.Home = world;
        _homeViewController.Init(GameState.Home);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        //  attempt to save the game state if we are pausing and have a legit game state.
        //  we don't want to bother saving the FTUE / bootstrapping state
        if (pauseStatus && GameState != null && GameState.Home != BootstrapWorld)
            _saveLoadController.Save(GameState);
    }

    private GameState CreateBootstrapGameState(bool devSettingsEnabled)
    {
        return new GameState
        {
            CurrentExpedition = new Expedition
            {
                CmdShip = BootstrapShip,
                Room = (Room)RoomFactory.GenerateBootstrapRoom(!devSettingsEnabled),
                CurrentMission = new Mission { MissionLevel = 0 },
            },
            Home = BootstrapWorld
        };
    }

    private static Homeworld _bootstrapWorld;
    private static Homeworld BootstrapWorld => _bootstrapWorld
        ?? (_bootstrapWorld = new Homeworld() { PlanetName = "???", Description = "???" });

    private static CommandShip _bootstrapShip;
    public static CommandShip BootstrapShip => _bootstrapShip
        ?? (_bootstrapShip = ShipFactory.GenerateCommandShip(RoomFactory));
}

public interface ISaveManager
{
    bool HasSaveFile { get; }
    SaveState SaveFile { get; }
    string SavePath { get; }
    void ResetSaveData();
}
