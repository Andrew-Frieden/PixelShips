using UnityEngine;
using Common;
using Events;
using TextSpace.Models;
using System.Collections.Generic;
using TextSpace.Events;
using TextSpace.Models.Actions;
using TextSpace.Services;
using TextSpace.Controllers;
using TextSpace.Services.Factories;
using TextSpace.Framework.IoC;

public class GameManager : Singleton<GameManager>, ISaveManager
{
	public enum GamePhase
	{
		BOOT,
		PREGAME,
		MISSION
	}

	public EventGameState OnGameStateChanged;

    private SaveLoadService SaveLoadSvc => ServiceContainer.Resolve<SaveLoadService>();
	private ContentLoadService ContentLoadSvc => ServiceContainer.Resolve<ContentLoadService>();
    private RoomFactoryService RoomFactory => ServiceContainer.Resolve<RoomFactoryService>();
    private ShipFactoryService ShipFactory => ServiceContainer.Resolve<ShipFactoryService>();
    private BootstrapService BootStrapSvc => ServiceContainer.Resolve<BootstrapService>();

    //  TODO refactor this hacky thing. should be event driven?
    [SerializeField] private CommandViewController _commandViewController;
	[SerializeField] private HomeViewController _homeViewController;

    private GamePhase _currentGamePhase = GamePhase.BOOT;
	public GamePhase CurrentGamePhase => _currentGamePhase;

    private GameState _gameState;
	public GameState GameState
    {
        get
        {
            if (_gameState == null)
                _gameState = new GameState();
            return _gameState;
        }
        set
        {
            _gameState.CurrentExpedition = value.CurrentExpedition;
            _gameState.Home = value.Home;
        }
    }

	private void Start()
	{
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        ServiceContainer.AddDependency(GameState);
        ServiceContainer.Construct();

        ServiceContainer.Resolve<SaveLoadService>().Init();
        _homeViewController.InitContentLoadResults();

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
    public bool HasSaveFile =>SaveLoadSvc.HasSaveData;
    public SaveState SaveFile => SaveLoadSvc.SaveData;
    public string SavePath => SaveLoadService.SaveFilePath;
    public void ResetSaveData()
    {
        SaveLoadSvc.Delete();
    }

    public void ContinueFromSave()
    {
        GameState = SaveLoadSvc.Load();
        UpdateState(GamePhase.MISSION);

        //  TODO refactor this hacky thing. Should be event driven probs?
        _commandViewController.StartCommandView();
        _homeViewController.Init(GameState.Home);
    }

    public void BootstrapNewGame(bool devSettingsEnabled = false)
	{
        bootstrapping = true;
        GameState = devSettingsEnabled ? BootStrapSvc.DevGameState : BootStrapSvc.GameState;
        UpdateState(GamePhase.MISSION);
        _homeViewController.Init(GameState.Home);
    }

    public void StartNewExpedition()
    {
        //  for the quick dev start, replace the bootstrapping world with a placeholder dev one
        if (GameState.Home == BootStrapSvc.DevGameState.Home)
            SetHomeworld(new Homeworld() { PlanetName = "Earth", Description = "Developer" });

        GameState.Home.ExpeditionCount++;
        GameState.CurrentExpedition = new Expedition
        {
            CmdShip = ShipFactory.GenerateCommandShip(),
            Room = (Room)RoomFactory.GenerateHomeworldRoom(GameState.Home),
            CurrentMission = null
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
        if (pauseStatus && GameState != null && GameState.Home != BootStrapSvc.DevGameState.Home)
            SaveLoadSvc.Save(GameState);
    }
}



public interface ISaveManager
{
    bool HasSaveFile { get; }
    SaveState SaveFile { get; }
    string SavePath { get; }
    void ResetSaveData();
}
