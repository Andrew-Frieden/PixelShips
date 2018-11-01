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

public class GameManager : Singleton<GameManager>
{
	public enum GamePhase
	{
		BOOT,
		PREGAME,
		MISSION
	}

	public EventGameState OnGameStateChanged;

    private SaveLoadService SaveLoadSvc => ServiceContainer.Resolve<SaveLoadService>();
    private BootstrapService BootStrapSvc => ServiceContainer.Resolve<BootstrapService>();
    private ExpeditionFactoryService ExpSvc => ServiceContainer.Resolve<ExpeditionFactoryService>();

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
            _gameState.Expedition = value.Expedition;
            _gameState.Home = value.Home;
        }
    }

	private void Start()
	{
        ServiceContainer.AddDependency(GameState);
        ServiceContainer.Construct();

        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

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

    public void ContinueFromSave()
    {
        GameState = SaveLoadSvc.Load();
        UpdateState(GamePhase.MISSION);

        //  TODO refactor this hacky thing. Should be event driven probs?
        _commandViewController.StartCommandView();
        UIResponseBroadcaster.Broadcast(UIResponseTag.UpdateHomeworld);

        //_homeViewController.Init(GameState.Home);
    }

    public void BootstrapNewGame(bool devSettingsEnabled = false)
	{
        bootstrapping = true;
        GameState = devSettingsEnabled ? BootStrapSvc.DevGameState : BootStrapSvc.FTUEGameState;
        UpdateState(GamePhase.MISSION);
        //_homeViewController.Init(GameState.Home);
    }

    public void StartNewExpedition()
    {
        GameState.Home.ExpeditionCount++;
        GameState.Expedition = ExpSvc.CreateNewExpedition();
        _commandViewController.StartCommandView();
        UIResponseBroadcaster.Broadcast(UIResponseTag.UpdateHomeworld);
    }

    public void SetHomeworld(Homeworld world)
    {
        GameState.Home = world;
        //_homeViewController.Init(GameState.Home);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        //  we don't want to bother saving the FTUE / bootstrapping state
        if (pauseStatus)
            SaveLoadSvc.Save(GameState);
    }
}
